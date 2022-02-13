using a7D.PDV.EF;
using a7D.PDV.EF.Enum;

namespace a7D.PDV.BLL
{
    public class ConfiguracoesIFood : ConfiguracaoBD
    {
        public ConfiguracoesIFood()
            : base((int)ETipoPDV.IFOOD, null)
        {
        }

        [Config("Ligar Integração iFood no Integrador", ETipoPDV.IFOOD, Valor = "0", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool IntegracaoIFood { get; set; }

        [Config("ID PDV para o Caixa do iFood", ETipoPDV.IFOOD, EConfig.ChaveUsuario, Valor = "", Obrigatorio = true)]
        public int CaixaPDV { get; set; }

        [Config("Chave do usuário a ser usado no iFood", ETipoPDV.IFOOD, EConfig.ChaveUsuario, Valor = "9933", Obrigatorio = true)]
        public string ChaveUsuario { get; set; }

        [Config("Habilitar venda no iFood", ETipoPDV.IFOOD, EConfig.HabilitarIFood, Valor = "1", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool HabilitarIFood { get; set; }

        [Config("Aprovar vendas do iFood automaticamente", ETipoPDV.IFOOD, EConfig.AprovarIFood, Valor = "1", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool AprovarIFood { get; set; }

        [Config("Estabeleciomento (Merchant_ID)", ETipoPDV.IFOOD)]
        public string merchant_id { get; set; }

        [Config("iConnect Username", ETipoPDV.IFOOD)]
        public string username { get; set; }

        [Config("iConnect Password", ETipoPDV.IFOOD)]
        public string password { get; set; }

        [Config("Atualizar Preços", ETipoPDV.IFOOD, Valor = "NAO", ValoresAceitos = "NAO:Não atualizar|V1:Valor 1|V2:Valor 2|V3:Valor 3", Obrigatorio = true)]
        public string AtualizaValor { get; set; }

        [Config("Atualizar Disponibilidade", ETipoPDV.IFOOD, Valor = "0", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool AtuaizarDisponibilidade { get; set; }
    }
}