using a7D.PDV.EF;
using a7D.PDV.EF.Enum;

namespace a7D.PDV.BLL
{
    public class ConfiguracoesLoggi : ConfiguracaoBD
    {
        public ConfiguracoesLoggi()
            : base((int)ETipoPDV.LOGGI, null)
        {
        }

        [Config("Ligar Integração Loggi no Caixa", ETipoPDV.LOGGI, Valor = "0", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool IntegracaoLoggi { get; set; }

        [Config("Usuário (email)", ETipoPDV.LOGGI, Valor = "")]
        public string EmailUsuario { get; set; } // email com inicio 'staging:' é para ambiente de testes

        [Config("Token de Acesso", ETipoPDV.LOGGI, Valor = "")]
        public string TokenUsuario { get; set; }

        [Config("Endereço de Origem", ETipoPDV.LOGGI, Valor = "Rua Nome, 123 bairro", Obrigatorio = true)]
        public string OrigemPedido { get; set; }

        [Config("Nome do Meio de Pagamento", ETipoPDV.LOGGI, Valor = "Padrão", Obrigatorio = true)]
        public string MeioPagamento { get; set; }
    }
}