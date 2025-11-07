using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using a7D.PDV.Integracao.Pagamento.StoneTEF.Dto;

namespace a7D.PDV.Integracao.Pagamento.StoneTEF
{
    public class AutoTefApiTEF : ITEF
    {
        public string Mensagem { get; private set; }
        public string Autorizacao { get; private set; } // ATK
        public string Bandeira { get; private set; } // brandName
        public string Adquirente { get; } = "Stone";
        public bool Debito { get; private set; }
        public string ViaEstabelecimento { get; private set; }
        public string ViaCliente { get; private set; }
        public Exception Erro { get; private set; }
        public decimal Valor { get; }

        public bool PrecisaSelecionar
        {
            get
            {
                return true;
            }
        }

        public bool PagamentoConfirmado
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Autorizacao);
            }
        }

        public string Log
        {
            get
            {
                return _log.ToString();
            }
        }

        readonly AutoTefClient _client;
        readonly StringBuilder _log = new StringBuilder();
        readonly int _pedidoId;
        MetodoPagamento _metodo = MetodoPagamento.Debito;
        int _parcelas = 1;
        readonly bool _hasAlcoholicDrink;

        string _panMask;
        int _transactionType; // 1=deb, 2=cred, 7=voucher

        Task _process;

        public AutoTefApiTEF(AutoTefClient client, int pedidoId, decimal valor, bool hasAlcoholicDrink)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _pedidoId = pedidoId;
            Valor = valor;
            _hasAlcoholicDrink = hasAlcoholicDrink;
        }

        void AddLog(string s)
        {
            _log.AppendLine($"{DateTime.Now:HH:mm:ss} {s}");
        }

        public void DefinirMetodoPagamento(MetodoPagamento metodo, int parcelas)
        {
            _metodo = metodo;
            _parcelas = Math.Max(1, parcelas);
            Debito = metodo == MetodoPagamento.Debito || metodo == MetodoPagamento.VoucherVR;
        }

        public ITEF IniciaVenda()
        {
            return this;
        }

        public bool Processando()
        {
            if (_process == null)
            {
                _process = Task.Run(async () =>
                {
                    try
                    {
                        Mensagem = "Autorizando...";
                        AddLog(Mensagem);

                        var accountType =
                            _metodo == MetodoPagamento.Debito ? "debit" :
                            _metodo == MetodoPagamento.Credito ? "credit" :
                            _metodo == MetodoPagamento.VoucherVR ? "voucher" :
                            "undefined";

                        AutoTefClient.Installment installment = null;
                        // if (accountType == "Credit")
                        // {
                            // 1 = À vista, 2 = Merchant, 3 = Issuer
                            var type =  1;
                            var number = 0;
                            installment = new AutoTefClient.Installment(type, number);
                        // }

                        var req = new AutoTefClient.PayRequest
                        {
                            amount = Valor,
                            accountType = accountType,
                            installment = installment,
                            hasAlcoholicDrink = _hasAlcoholicDrink,
                            splits = null
                        };

                        using (var resp = await _client.PayAsync(req).ConfigureAwait(false))
                        {
                            var text = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);

                            if (!resp.IsSuccessStatusCode)
                                throw new Exception($"AutoTEF Pay falhou ({(int)resp.StatusCode}): {text}");

                            var data = JsonSerializer.Deserialize<PayResponse>(text,
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                            if (data?.receipt?.acquirerTransactionKey == null)
                                throw new Exception($"Pagamento não aprovado: {data?.messageDisplay ?? "sem detalhes"}");

                            // Mapear campos de retorno
                            Autorizacao = data.receipt.acquirerTransactionKey; // ATK
                            Bandeira = data.brandName;
                            ViaCliente = data.receipt.clientVia;
                            ViaEstabelecimento = data.receipt.merchantVia;
                            Mensagem = data.messageDisplay ?? "Aprovado";
                            _panMask = data.card?.maskedPrimaryAccountNumber;
                            _transactionType = data.transactionType;

                            AddLog($"APROVADO ATK={Autorizacao} {Bandeira} VALOR={Valor:N2}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Erro = ex;
                        Mensagem = ex.Message;
                        AddLog("ERRO: " + ex.Message);
                    }
                    finally
                    {
                        AddLog("Fim");
                    }
                });
            }

            // retorna true enquanto a task estiver rodando
            return !_process.IsCompleted;
        }

        public async Task AguardaTransacao(StatusUpdateCallBack respostaTEF = null)
        {
            if (_process != null)
                await _process.ConfigureAwait(false);

            respostaTEF?.Invoke(this);
        }

        public void Cancelar()
        {
            Mensagem = "Cancelando (operador)...";
            AddLog(Mensagem);
        }

        public void Estornar()
        {
            //  await CancelarComApiAsync(_client, Autorizacao, Valor, _transactionType, _panMask);
        }

        public static async Task<bool> CancelarComApiAsync(AutoTefClient client, string atk, decimal amount, int transactionType,
            string panMask)
        {
            var req = new AutoTefClient.CancelRequest
            {
                acquirerTransactionKey = atk,
                amount = amount,
                transactionType = transactionType, // 1/2/7
                panMask = panMask,
                splits = null
            };

            using (var resp = await client.CancelAsync(req).ConfigureAwait(false))
            {
                return resp.IsSuccessStatusCode;
            }
        }
    }
}
