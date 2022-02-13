using a7D.PDV.EF;
using a7D.PDV.EF.Enum;

namespace a7D.PDV.BLL
{
    public class ConfiguracoesEasyChopp : ConfiguracaoBD
    {
        public ConfiguracoesEasyChopp()
            : base((int)ETipoPDV.EASYCHOPP, null)
        {
        }

        [Config("Ligar Integração EasyChopp no Integrador", ETipoPDV.EASYCHOPP, Valor = "0", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool IntegracaoEasyChopp { get; set; }

        [Config("URL API", ETipoPDV.EASYCHOPP, Valor = "", Obrigatorio = true)]
        public string URLBase { get; set; }

        [Config("Chave de Segurança", ETipoPDV.EASYCHOPP, Valor = "", Obrigatorio = true)]
        public string ChaveSeguranca { get; set; }

        [Config("E-mail de usuário", ETipoPDV.EASYCHOPP, Valor = "", Obrigatorio = true)]
        public string EmailUsuario { get; set; }

        [Config("ID da categoria com produtos de crédito", ETipoPDV.EASYCHOPP, Valor = "0", Obrigatorio = true)]
        public int CategoriaCredito { get; set; }
    }
}