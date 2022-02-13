using System;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.Pagamento.GranitoTEF
{
    public class GranitoPinpad : ITEF
    {
        public string Mensagem { get; private set; }
        public string Autorizacao { get; private set; }
        public string Bandeira { get; private set; }
        public string Adquirente { get; private set; }
        public bool Debito { get; private set; }
        public string ViaEstabelecimento { get; private set; }
        public string ViaCliente { get; private set; }
        public decimal Valor { get; private set; }
        public Exception Erro { get; private set; }
        public bool PrecisaSelecionar => true;

        private static bool Configure = false;
        private static bool Init = false;
        private string smsPara = null;
        private bool aguardar = true;

        public static string GranitoIdPDV;
        public static string GranitoCode;
        public static string GranitoCNPJ;

        private StringPdvResponse pdv;
        private StringBuilder tefBuffer;
        private StringTefPdvInit init;
        private StringBuilder log;

        public GranitoPinpad(int identificador, int documento, decimal valor, string celular)
        {
            log = new StringBuilder("Nova Venda: " + identificador);
            smsPara = celular;
            Valor = valor;
            init = new StringTefPdvInit()
            { 
                Identificador = identificador,
                Documento = documento,
                Valor = valor
            };
            pdv = new StringPdvResponse()
            {
                Tipo = 1,
                Identificador = init.Identificador
            };
            tefBuffer = new StringBuilder(10000);

            GranitoLogin.PDV = GranitoIdPDV;
            GranitoLogin.CNPJ = GranitoCNPJ;
            GranitoLogin.Senha = GranitoCode;

            if (!Configure)
                Mensagem = "Configurando";
        }

        public bool Processando()
        {
            try
            {
                if (!Configure)
                {
                    if (string.IsNullOrEmpty(GranitoLogin.CNPJ) || string.IsNullOrEmpty(GranitoLogin.Senha))
                    {
                        Mensagem = "Usuário e Senha PAGO não habilitado";
                        return false;
                    }

                    var result1 = GranitoWrapperAPI.PG_Config(GranitoLogin.CNPJ, GranitoLogin.Senha, GranitoLogin.PDV);
                    if (result1 == 0)
                        Configure = true;
                    else
                    {
                        Mensagem = "Erro ao inicializar: " + result1;
                        return false;
                    }

                    if (!Init)
                        Mensagem = "Iniciando";

                    return true; // Continuar
                }

                if (!Init)
                {
                    var result2 = GranitoWrapperAPI.PG_Init(new StringBuilder("PDVSeven\nTEF PAGO"), tefBuffer);
                    if (result2 == 0)
                        Init = true;
                    else
                    {
                        Mensagem = "Erro ao configurar: " + result2;
                        return false;
                    }

                    Mensagem = "Aguarde, processando...";

                    return true; // Continuar
                }

                // Para fins de testes
                //tefBuffer = new StringBuilder("1123456TRANSACAO APROVADA                                         010000611028500TRANSACAO APROVADA            00700311000000000000005003003496831482             MASTERCARD|         Via Estabelecimento|| PAGO SA| Estab: 0000240 Pdv: 003| CNPJ: 22.177.858/0001-69| ALAMEDA RIO NEGRO, 500| BARUERI - SP|| Cred a vista | Valor:                        R$ 0,05| NSU Host: 400027902018   Auto: 030923| NSU: 003004       NL: 012005504624078| 537110XXXXXX8017       11/12/17 18:42| Codigo Transacao: 496831| 000000000033|| ARQC: 9995020678345912| AID: A0000000041010| Credit||  TRANSACAO APROVADA MEDIANTE SENHA|            SOUZA/FABIO F||360             MASTERCARD|             Via Cliente|| PAGO SA| Estab: 0000240 Pdv: 003| CNPJ: 22.177.858/0001-69| ALAMEDA RIO NEGRO, 500| BARUERI - SP|| Cred a vista | Valor:                        R$ 0,05| NSU Host: 400027902018   Auto: 030923| NSU: 003004       NL: 012005504624078| 537110XXXXXX8017       11/12/17 18:42| Codigo Transacao: 496831| 000000000033|");
                //var result = (int)PG_Retorno.TRN_OK;

                log.AppendLine(">>>>>> " + pdv.Parametro);
                var result = GranitoWrapperAPI.PG_Tef(init, pdv, tefBuffer);
                log.AppendLine($"<<<[{result}] {tefBuffer.ToString()}");

                if (result == (int)PG_Retorno.END)
                {
                    log.AppendLine("FIM");
                    return false; // Finalizar Espera
                }
                else if (result == (int)PG_Retorno.PROCESSING)
                {
                    var tef = new StringTefResult(tefBuffer);
                    pdv.Tipo = tef.Tipo;

                    if (pdv.Tipo == 3)
                        pdv.DadoColetado = "2";
                    else
                        Mensagem = tef.Mensagem;
                }
                else if (result == (int)PG_Retorno.TRN_OK)
                {
                    var tef = new StringTefAprovado(tefBuffer);
                    pdv.Tipo = tef.Tipo;
                    Mensagem = "AUTORIZADO: " + tef.Aturorizacao;
                    Autorizacao = tef.Aturorizacao;
                    Adquirente = tef.Adiquirente.ToString();
                    Bandeira = tef.Bandeira.ToString();
                    Debito = tef.Transacao == PG_TipoTransacao.Debito;
                    ViaEstabelecimento = tef.CuponEstabelecimento;
                    ViaCliente = tef.CuponCliente;

                    if (smsPara != null)
                    {
                        var send = new StringPdvSMS(tef.Tipo, pdv.Identificador, smsPara);

                        log.AppendLine(">>>>>> " + send.Parametro);
                        var resultSMS = GranitoWrapperAPI.PG_Tef(init, send, tefBuffer);
                        log.AppendLine($"<<<[{result}] {tefBuffer.ToString()}");

                        var tef2 = new StringTefResult(tefBuffer);
                        pdv.Tipo = tef2.Tipo;
                    }
                }
                else //OK ou NOK
                {
                    var tef = new StringTefResult(tefBuffer);
                    Mensagem = tef.Mensagem;
                    if (tef.Tipo == 5)
                        return false; // Finaliza
                }
                return true; // Continuar
            }
            catch (Exception ex)
            {
                Mensagem = "Erro no processamento\r\n" + ex.Message;
                Erro = ex;
                return false;
            }
        }

        public string Log => log.ToString();

        public bool PagamentoConfirmado => Autorizacao != null && Autorizacao.Length > 0;

        public ITEF IniciaVenda()
        {
            return this;
        }

        public async Task AguardaTransacao(StatusUpdateCallBack respostaTEF)
        {
            bool emProcesso;
            do
            {
                emProcesso = Processando();
                respostaTEF?.Invoke(this);
                await Task.Delay(250);
            } while (emProcesso && aguardar);
        }

        public void Cancelar()
        {
            Mensagem = "Cancelado";
            aguardar = false;
        }

        public void Estornar()
        {
        }

        public void DefinirMetodoPagamento(MetodoPagamento metodo, int parcelas)
        {
            init.Transacao = metodo == MetodoPagamento.Debito ? PG_TipoTransacao.Debito : PG_TipoTransacao.Credito;
            init.Parcelas = parcelas;
        }
    }
}