using MuxxLib;
using MuxxLib.Enum_s;
using MuxxLib.Interface;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.Pagamento.NTKTEFDLL
{
    public class NTKPinpadPayGoWeb : ITEF
    {
        private PGWebLib lib;
        private PagamentoResultado pagamento;

        public bool Confirmado => pagamento.Confirmado;
        public Exception Erro => lib.Erro;
        public string Bandeira => pagamento?.Bandeira;
        public string Adquirente => pagamento?.ContaRecebivel;
        public string Autorizacao => pagamento?.Autorizacao;
        public string ViaEstabelecimento => pagamento?.ViaEstabelecimento;
        public string ViaCliente => pagamento?.ViaCliente;
        public decimal Valor { get; private set; }
        public string Mensagem { get; private set; }
        public bool PrecisaSelecionar => Operacao == TipoOperacao.Venda;
        public bool Debito { get; private set; }
        public TipoOperacao Operacao { get; set; }
        public string Recibo { get; private set; }
        public string Log => null;
        private MetodoPagamento Metodo;

        public int Parcelas { get; private set; }

        public bool PagamentoConfirmado => throw new NotImplementedException();

        private bool VendaIniciada;
        private string versao;
        private StatusUpdateCallBack respostaTEF;
        private int parte;
        private ushort parte3Count;
        private ITipoDeDado parte4Dado;

        private void DefineMensagem(string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                if (msg.StartsWith("\r"))
                    msg = msg.Substring(1);

                Mensagem = msg.Trim();
            }
        }

        public NTKPinpadPayGoWeb(int identificador, int documento, decimal valor, FactoryBase factory)
        {
            this.Valor = valor;
            Mensagem = "Inicializando";
            versao = Assembly.GetCallingAssembly().ManifestModule.Assembly.ToString().Replace(", Culture=neutral, PublicKeyToken=null", "").Split('=')[1];
            lib = new PGWebLib(factory);
            if (!lib.Init())
            {
                Mensagem = $"Erro na inicialização: {lib.Erro?.Message}";
            }
            //else if (lib.Display(AutomacaoConstantes.Nome, out string retorno))
            //{
            //    if (!string.IsNullOrEmpty(retorno))
            //        Mensagem = retorno;
            //}

            parte = 1;
        }

        public void DefinirMetodoPagamento(MetodoPagamento metodo, int parcelas)
        {
            Mensagem = "Aguarde";
            Metodo = metodo;
            Parcelas = parcelas;
            Debito = metodo == MetodoPagamento.Debito;
        }

        private bool Parte1CriaTrancacao()
        {
            if (Operacao == TipoOperacao.ConfirmarPendente)
            {
                EfetivarPendente();
                return false;
            }
            else if (Operacao == TipoOperacao.CancelarPendente)
            {
                CancelarPendente();
                return false;
            }
            if (!lib.NewTransac(Operacao == TipoOperacao.Administrar ? PWOPER.Admin : PWOPER.Sale))
            {
                if (lib.Result == PWRET.NOTINST && Operacao != TipoOperacao.Administrar)
                {
                    // Na primeira tentativa de pagamento em um novo ambiente não instalado, tenta instalar
                    Operacao = TipoOperacao.Administrar;
                    return true;
                }

                return false;
            }

            VendaIniciada = Operacao != TipoOperacao.Venda; // Se não é venda irá iniciar os parametros da venda
            parte = 2;
            return
                lib.AddParam(PWINFO.AUTDEV, AutomacaoConstantes.Empresa) &&
                lib.AddParam(PWINFO.AUTVER, versao) &&
                lib.AddParam(PWINFO.AUTNAME, AutomacaoConstantes.Nome) &&
                lib.AddParam(PWINFO.AUTCAP, "31");
        }


        public bool Parte2BuscaParametros()
        {
            //Thread.Sleep(500);
            if (!lib.ExecTransac())
            {
                Mensagem = "Transação não iniciada";
                return false;
            }
            else if (lib.Result == PWRET.MOREDATA)
            {
                if (!VendaIniciada)
                {
                    VendaIniciada = true;
                    bool ok =
                        lib.AddParam(PWINFO.TOTAMNT, ((int)(Valor * 100)).ToString()) &&
                        lib.AddParam(PWINFO.CURRENCY, Constantes.CODIGOMOEDA) &&
                        lib.AddParam(PWINFO.CURREXP, Constantes.EXPOENTEMOEDA);

                    if (ok && Metodo != MetodoPagamento.Selecione)
                        ok = lib.AddParam(PWINFO.CARDTYPE, Debito ? "2" : "1");

                    if (ok)
                    {
                        if (Debito || Parcelas == 1)
                            ok = lib.AddParam(PWINFO.FINTYPE, "1"); // A Vista Loja
                        else
                            ok = lib.AddParam(PWINFO.FINTYPE, "2"); // Parcelado Loja
                    }

                    if (!ok)
                        return false;
                }
                else
                {
                    parte3Count = 0;
                    parte = 3;
                }
                return true;
            }
            else if (lib.Result == PWRET.OK)
            {
                string cFile = null;
                if (Operacao == TipoOperacao.Administrar)
                {
                    Recibo = lib.GetResult(PWINFO.RCPTFULL);
                    if (!string.IsNullOrEmpty(Recibo))
                        Recibo += "\r-----\r";

                    Recibo += lib.GetResult(PWINFO.RCPTMERCH);
                    if (!string.IsNullOrEmpty(Recibo))
                        Recibo += "\r-----\r";

                    Recibo += lib.GetResult(PWINFO.RCPTCHOLDER);

                    if (string.IsNullOrEmpty(Recibo))
                        Mensagem = "Nada a imprimir";
                }
                else
                {
                    pagamento = lib.ObterResultado(out cFile); // Transação realizada, tem que obter os dados
                    Recibo =
                        "\r\nViaCliente:\r\n" +
                        pagamento.ViaCliente +
                        "\r\nViaEstabelecimento:\r\n" +
                        pagamento.ViaEstabelecimento;
                }

                if (lib.GetResult(PWINFO.CNFREQ) == "1")
                    lib.Confirmation(PWCNF.PWCNF_CNF_AUTO, pagamento, cFile); // Confirma transação
            }
            else if (lib.Result == PWRET.PINPADERR)
                lib.ResetDLL();

            return false;
        }

        public bool Parte3ProcessaParametros()
        {
            if (parte3Count >= lib.NumParam)
            {
                parte = 2;
                return true;
            }

            Mensagem = "Aguarde";
            parte4Dado = lib.CriaOperacao(parte3Count);
            parte = 4;
            return true;
        }

        public bool Parte4ProcessaDados()
        {
            string msg = "";
            var resultOperation = lib.ExecutaOperacao(parte4Dado, parte3Count, ref msg);
            DefineMensagem(msg);

            //DefineMensagem(lib.GetResult(PWINFO.RESULTMSG));

            respostaTEF?.Invoke(this);
            if (resultOperation == PWRET.DISPLAY || resultOperation == PWRET.NOTHING)
                return true;

            parte = 3;
            parte3Count++;
            return resultOperation != PWRET.CANCEL;
        }

        public bool ProcessandoEtapa()
        {
            if (lib.Result == PWRET.CANCEL)
                return false;
            else if (parte == 1)
                return Parte1CriaTrancacao();
            else if (parte == 2)
                return Parte2BuscaParametros();
            else if (parte == 3)
                return Parte3ProcessaParametros();
            else if (parte == 4)
                return Parte4ProcessaDados();
            else
                return false;
        }

        public bool Processando()
        {
            if (ProcessandoEtapa())
                return true;
            else
            {
                if (Operacao == TipoOperacao.Venda && pagamento == null)
                    Interop.PW_iPPAbort();

                DefineMensagem(lib.GetResult(PWINFO.RESULTMSG));

                return false;
            }
        }

        public Task IniciaTransacao()
        {
            return Task.Factory.StartNew(() =>
            {
                while (Processando())
                {
                    respostaTEF?.Invoke(this);
                    Thread.Sleep(100);
                }
                respostaTEF?.Invoke(this);
            });
        }

        public async Task AguardaTransacao(StatusUpdateCallBack callBackRespostaTEF)
        {
            respostaTEF = callBackRespostaTEF;
            await Task.Factory.StartNew(() =>
            {
                Task.WaitAll(IniciaTransacao());  //AGUARDA A CONCLUSÇÃO DA TRANSAÇÃO
            });
        }

        public void Cancelar()
        {
            Mensagem = "Cancelado";
            Interop.PW_iPPAbort();
        }

        public void Estornar()
        {
        }

        public bool Confirmar(string cFileAUT)
        {
            var pagamento = PGWebLib.LerResultado(cFileAUT);
            return lib.Confirmation(PWCNF.PWCNF_CNF_AUTO, pagamento, cFileAUT);
        }

        public string Pendente()
        {
            return lib.Pendente();
        }

        private void EfetivarPendente()
        {
            pagamento = new PagamentoResultado()
            {
                Autorizacao = lib.GetResult(PWINFO.PNDREQNUM),
                LocRef = lib.GetResult(PWINFO.PNDAUTLOCREF),
                ExtRef = lib.GetResult(PWINFO.PNDAUTEXTREF),
                VirtMerch = lib.GetResult(PWINFO.PNDVIRTMERCH),
                ContaRecebivel = lib.GetResult(PWINFO.PNDAUTHSYST)
            };
            if (string.IsNullOrEmpty(pagamento.Autorizacao))
                Recibo = Mensagem = "Sem transação pendente";
            else
            {
                if (lib.Confirmation(PWCNF.PWCNF_CNF_MANU_AUT, pagamento, null))
                    Mensagem = "Transação pendente confirmada";
                else
                    Mensagem = "Erro ao confirmar a transação pendente";

                Recibo = Mensagem;
            }
        }

        private void CancelarPendente()
        {
            pagamento = new PagamentoResultado()
            {
                Autorizacao = lib.GetResult(PWINFO.PNDREQNUM),
                LocRef = lib.GetResult(PWINFO.PNDAUTLOCREF),
                ExtRef = lib.GetResult(PWINFO.PNDAUTEXTREF),
                VirtMerch = lib.GetResult(PWINFO.PNDVIRTMERCH),
                ContaRecebivel = lib.GetResult(PWINFO.PNDAUTHSYST)
            };
            if (string.IsNullOrEmpty(pagamento.Autorizacao))
                Recibo = Mensagem = "Sem transação pendente";
            else
            {
                if (lib.Confirmation(PWCNF.PWCNF_REV_MANU_AUT, pagamento, null))
                    Mensagem = "Transação pendente cancelada";
                else
                    Mensagem = "Erro ao cancelar a transação pendente";

                Recibo = Mensagem;
            }
        }
    }
}