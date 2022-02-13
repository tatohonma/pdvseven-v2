using Communication.Sdk.Exceptions;
using Microtef.Core;
using Microtef.Core.Authorization;
using Microtef.Core.Staging;
using Pinpad.Sdk.Model;
using Pinpad.Sdk.Model.Exceptions;
using Poi.Sdk.Authorization.Report;
using Poi.Sdk.Authorization.TypeCode;
using Poi.Sdk.Exceptions;
using System;
using System.IO;

namespace a7D.PDV.Integracao.Pagamento.StoneTEF
{
    /// <summary>
    /// <see cref="ICardPaymentAuthorizer"/> wrapper, responsible for connect to 
    /// the pinpad, transaction operation, cancelation, show transactions on the
    /// console and closing connection with the pinpads.
    /// </summary>
    public static class AuthorizationCore
    {
        public static string logPath;

        /// <summary>
        /// All transactions tried (approved, not approved and canceled).
        /// </summary>
        /// private ICollection<TransactionTableEntry> Transactions { get; set; }

        /// <summary>
        /// Stone authorizer core.
        /// </summary>
        private static ICardPaymentAuthorizer StoneAuthorizer { get; set; }

        /// <summary>
        /// If the <see cref="Instance"/> is eligible to use.
        /// </summary>
        public static bool IsUsable { get { return StoneAuthorizer == null ? false : true; } }

        /// <summary>
        /// Static constructor to create the <see cref="Instance"/>.
        /// </summary>
        private static void InitTest()
        {
            //O StoneCode "185346049" de teste só funciona neste ambiente

            // Setup integration environment. Comment the lines below if you want to process a transaction in the production endpoint:
            // If it's false, the authorizer wil always point to the production endpoint:
            FallbackSettings.EnableFallback = true;

            // Integration endpoint to the authorizer
            FallbackSettings.FallbackAuthorizerUri = "https://sandbox-auth-integration.stone.com.br/";

            // Integration endpoint to TMS (Terminal Management System), responsible for updating pinpad tables with supported brands and cards
            FallbackSettings.FallbackTmsUri = "https://tms-staging.stone.com.br/";

            // Integration endpoint to PHC (Poi Host Communication)
            FallbackSettings.FallbackPhcUri = "https://poihostcommunication-stg.stone.com.br/";
        }

        public static void Reset()
        {
            StoneAuthorizer = null;
        }

        /// <summary>
        /// Try to connect to the pinpad.
        /// </summary>
        public static bool TryActivate(string stoneCode, string wellcome)
        {
            try
            {
                if (IsUsable)
                    return true;

                logPath = new FileInfo(typeof(AuthorizationCore).Assembly.Location).Directory.FullName;
                if (!Directory.Exists(logPath))
                    Directory.CreateDirectory(logPath);

                var result = Microtef.Platform.Desktop.DesktopInitializer.Initialize();
                
                // InitTest(); !!! CUIDADO Só use em homologação

                var display = new DisplayableMessages()
                {
                    MainLabel = wellcome
                };
                StoneAuthorizer = DeviceProvider.ActivateAndGetOneOrFirst(stoneCode, display);

                logStatus($"STONECODE {stoneCode} {StoneAuthorizer.Merchant.CompanyName} {StoneAuthorizer.Merchant.IdentityCode} {StoneAuthorizer.Merchant.Address}");

                return IsUsable;
            }
            catch(InvalidStoneCodeException ex)
            {
                logStatus(ex);
                throw new Exception("Stone Code inválido.", ex);
            }
            catch (PinpadNotFoundException ex)
            {
                logStatus(ex);
                throw new Exception("Pinpad nao encontrado.", ex);
            }
            catch (Exception ex)
            {
                logStatus(ex);
                throw new Exception("Erro ao ativar o terminal. Você está usando o StoneCode correto? " + ex.Message);
            }
        }

        /// <summary>
        /// Authorizes a payment.
        /// </summary>
        /// <returns>The report returned from Stone Authorizer, or null if something 
        /// went wrong.</returns>
        public static PagamentoResultado Authorize(int id, decimal valor, bool debito, string email, Action<string> status)
        {
            // Verify if the authorizer is eligible to do something:
            if (IsUsable == false) { return null; }

            // Setup transaction data:
            ITransactionEntry transactionEntry = new TransactionEntry
            {
                Amount = valor,
                CaptureTransaction = true,
                InitiatorTransactionKey = id.ToString(),
                Type = debito ? AccountType.Debit : AccountType.Credit,
            };

            try
            {
                // Authorize the transaction setup and return it's value:
                StoneAuthorizer.OnStateChanged += (s, e) =>
                {
                    string info = StatusTranslate(e);
                    logStatus(info);
                    status(info);
                };

                logStatus($"AUTH {transactionEntry.Type} {transactionEntry.Amount.ToString("N2")} ID {transactionEntry.InitiatorTransactionKey}");

                var auth = StoneAuthorizer.Authorize(transactionEntry, out ResponseStatus authorizationStatus);

                logStatus($"END {auth.TransactionType} {auth.Amount.ToString("N2")} {auth.Card?.MaskedPrimaryAccountNumber} ATK {auth.AcquirerTransactionKey} {auth.Receipt?.BrandName} {auth.Receipt?.CardholderName}");

                // Show result on console:
                if (auth.WasSuccessful != true)
                    return null;

                if (!string.IsNullOrEmpty(email))
                {
                    try
                    {
                        status("enviando comprovante para: " + email);
                        auth.SendAuthorizationReceipt(StoneAuthorizer.Merchant, email);
                    }
                    catch (Exception ex)
                    {
                        logStatus(ex, true);
                    }
                }

                return new PagamentoResultado()
                {
                    Autorizacao = auth.AcquirerTransactionKey,
                    Bandeira = auth.Receipt.BrandName,
                    Debito = auth.TransactionType == AccountType.Debit,
                    ContaRecebivel = "Stone",
                    ViaEstabelecimento = ViaEstabelecimento(auth),
                    ViaCliente = ViaCliente(auth)
                };
            }
            catch (CardHasChipException ex)
            {
                logStatus(ex);
                throw new Exception("O cartao possui chip. For favor, insira-o.", ex);
            }
            catch (ExpiredCardException ex)
            {
                logStatus(ex);
                throw new Exception("Cartão expirado.", ex);
            }
            catch (InvalidConnectionException ex)
            {
                logStatus(ex);
                throw new Exception("Sem conexão.", ex);
            }
            catch (InvalidStoneCodeException ex)
            {
                logStatus(ex);
                throw new Exception("Stone Code inválido.", ex);
            }
            catch (Exception ex)
            {
                logStatus(ex, true);
                throw new Exception("Ocorreu um erro na transacao: " + ex.Message, ex);
            }
        }

        private static void logStatus(Exception ex, bool stacktrace = false)
        {
            logStatus($"{ex.GetType()}: {ex.Message}{(stacktrace ? $"\r\n{ex.StackTrace}\r\n" : "")}");
        }

        private static void logStatus(string info)
        {
            try
            {
                string file = Path.Combine(logPath, $"StoneTEF-{DateTime.Now.ToString("yyyy-MM-dd")}.txt");
                File.AppendAllText(file, $"{DateTime.Now.ToString("HH:mm:ss")} {info}\r\n");
            }
            catch (Exception)
            {
            }
        }

        private static string ViaCliente(IAuthorizationReport auth)
        {
            string comprovante = "";
            try
            {
                comprovante += ParValor("STONE", "via cliente") + "\r\n";
                comprovante += ParValor(StoneAuthorizer.Merchant.CompanyName, StoneAuthorizer.Merchant.IdentityCode) + "\r\n";
                comprovante += ParValor(StoneAuthorizer.Merchant.Address?.Street + ", " + StoneAuthorizer.Merchant.Address?.DoorNumber, StoneAuthorizer.Merchant.Address?.City) + "\r\n";
                comprovante += ParValor(auth.TransactionType == AccountType.Credit ? "CREDITO A VISTA" : "DEBITO", "") + "\r\n";
                comprovante += ParValor("TOTAL:", auth.Amount.ToString("N2")) + "\r\n";
                comprovante += ParValor(auth.Receipt.BrandName, auth.Card.MaskedPrimaryAccountNumber) + "\r\n";
                comprovante += ParValor("AUTO: " + auth.AcquirerTransactionKey, DateTime.Now.ToString("dd/MM/yyyy HH:mm")) + "\r\n";
            }
            catch (Exception)
            {
            }
            return comprovante;
        }

        private static string ViaEstabelecimento(IAuthorizationReport auth)
        {
            string comprovante = "";
            try
            {
                comprovante += ParValor("STONE", "via estabelecimento") + "\r\n";
                comprovante += ParValor(StoneAuthorizer.Merchant.CompanyName, StoneAuthorizer.Merchant.IdentityCode) + "\r\n";
                comprovante += ParValor(StoneAuthorizer.Merchant.Address?.Street + ", " + StoneAuthorizer.Merchant.Address?.DoorNumber, StoneAuthorizer.Merchant.Address?.City) + "\r\n";
                comprovante += ParValor(auth.TransactionType == AccountType.Credit ? "CREDITO A VISTA" : "DEBITO", "") + "\r\n";
                comprovante += ParValor("TOTAL:", auth.Amount.ToString("N2")) + "\r\n";
                comprovante += ParValor(auth.Receipt.BrandName, auth.Card.MaskedPrimaryAccountNumber) + "\r\n";
                comprovante += ParValor("AUTO: " + auth.AcquirerTransactionKey, DateTime.Now.ToString("dd/MM/yyyy HH:mm")) + "\r\n";
            }
            catch (Exception)
            {
            }
            return comprovante;
        }

        public static string ParValor(string direita, string esquerda)
        {
            // ------1234567890123456789012345678901234567890
            int cols = 36;
            if (direita.Length + esquerda.Length > cols)
            {
                if (esquerda.Length > cols / 2)
                    esquerda = esquerda.Substring(0, cols / 2);

                if (direita.Length > cols / 2)
                    direita = direita.Substring(0, cols / 2);
            }

            int espaco = cols - (direita.Length + esquerda.Length);
            return direita + new string(' ', espaco + 1) + esquerda;
        }

        public static string StatusTranslate(AuthorizationStatusChangeEventArgs e)
        {
            switch (e.AuthorizationStatus)
            {
                case AuthorizationStatus.Undefined:
                    return "Erro desconhecido";
                case AuthorizationStatus.InvalidSaleAffiliationKey:
                    return "Afiliação inválida";
                case AuthorizationStatus.InvalidTransaction:
                    return "Transação inválida";
                case AuthorizationStatus.TransactionCreated:
                    return "Criando transação";
                case AuthorizationStatus.Authorizing:
                    return "Autorizando";
                case AuthorizationStatus.Approved:
                    return "Aprovado";
                case AuthorizationStatus.NotApproved:
                    return "Negado";
                case AuthorizationStatus.PartiallyApproved:
                    return "Parcialmente aprovado";
                case AuthorizationStatus.TechnicalError:
                    return "Erro técnico";
                case AuthorizationStatus.UserCancelled:
                    return "Cancelado pelo usuário";
                case AuthorizationStatus.SystemCancelled:
                    return "Sistema cancelado";
                case AuthorizationStatus.AwaitingCard:
                    return "Aguardando cartão";
                case AuthorizationStatus.AwaitingPinCode:
                    return "Aguardando pinpad";
                case AuthorizationStatus.NetworkError:
                    return "Erro de rede";
                case AuthorizationStatus.CardReadingError:
                    return "Erro ao ler o cartão";
                case AuthorizationStatus.CardReadingFatalError:
                    return "Erro crítico ao ler o cartão";
                case AuthorizationStatus.PinpadGenericError:
                    return "Erro no pinpad";
                case AuthorizationStatus.PinpadNotFoundError:
                    return "Pinpad não encontrado";
                case AuthorizationStatus.AwaitingCardRemoval:
                    return "Retire o cartão";
                case AuthorizationStatus.TableDownloading:
                    return "Atualizando tabelas";
                case AuthorizationStatus.TableDownloadingError:
                    return "Erro ao atualizar tabelas";
                case AuthorizationStatus.TransactionFinished:
                    return "Transação finalizada";
                //case AuthorizationStatus.DumbCard:
                case AuthorizationStatus.InvalidCard:
                    return "Cartão inválido";
                case AuthorizationStatus.ConnectionError:
                    return "Erro de conexão";
                case AuthorizationStatus.CardNotFound:
                    return "Cartão não encontrado";
                case AuthorizationStatus.FinishingTransaction:
                    return "Finalizando transação";
                case AuthorizationStatus.TimeOut:
                    return "Tempo excedido";
                default:
                    return e.AuthorizationStatus.ToString();
            }
        }

        public static bool Cancel(string atk, decimal valor, out string responseCode)
        {
            var canc = StoneAuthorizer.Cancel(atk, valor);
            logStatus($"CANCEL {valor.ToString("N2")} {canc.WasSuccessful} {canc.AmountCanceled?.ToString("N2")} {canc.ResponseCode} {canc.ResponseReason}");

            responseCode = canc.ResponseCode;
            return canc.WasSuccessful;
        }

        /// <summary>
        /// Closes pinpad connection.
        /// </summary>
        public static void ClosePinpad()
        {
            StoneAuthorizer.PinpadFacade.Communication.ClosePinpadConnection(StoneAuthorizer.PinpadMessages.MainLabel);
        }

        public static void CancelPinpad()
        {
            StoneAuthorizer.PinpadFacade.Communication.CancelRequest();
        }
    }
}
