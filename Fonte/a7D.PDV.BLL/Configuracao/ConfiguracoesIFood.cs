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

        [Config("ID PDV para o Caixa do iFood", ETipoPDV.IFOOD, Valor = "", Obrigatorio = true)]
        public int CaixaPDV { get; set; }

        [Config("Chave do usuário a ser usado no iFood", ETipoPDV.IFOOD, EConfig.ChaveUsuario, Valor = "9933", Obrigatorio = true)]
        public string ChaveUsuario { get; set; }

        [Config("Habilitar venda no iFood", ETipoPDV.IFOOD, Valor = "1", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool HabilitarIFood { get; set; }

        [Config("Aprovar vendas do iFood automaticamente", ETipoPDV.IFOOD, Valor = "1", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool AprovarIFood { get; set; }

        [Config("Finalizar pedido no sistema automaticamente na finalização do iFood", ETipoPDV.IFOOD, Valor = "1", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool FinalizacaoAutomatica { get; set; }

        [Config("ClientId (PDVSeven)", ETipoPDV.IFOOD)]
        public string ClientId { get; set; }

        [Config("ClientSecret (PDVSeven)", ETipoPDV.IFOOD)]
        public string ClientSecret { get; set; }

        [Config("AuthorizationCodeVerifier (api)", ETipoPDV.IFOOD)]
        public string AuthorizationCodeVerifier { get; set; }

        [Config("AuthorizationCode (Portal Parceiro)", ETipoPDV.IFOOD)]
        public string AuthorizationCode { get; set; }

        [Config("RefreshToken (api)", ETipoPDV.IFOOD)]
        public string RefreshToken { get; set; }

        [Config("merchantId (Portal Parceiro)", ETipoPDV.IFOOD)]
        public string merchantId { get; set; }

         //[Config("Atualizar Preços", ETipoPDV.IFOOD, Valor = "NAO", ValoresAceitos = "NAO:Não atualizar|V1:Valor 1|V2:Valor 2|V3:Valor 3", Obrigatorio = true)]
        //public string AtualizaValor { get; set; }

        //[Config("Atualizar Disponibilidade", ETipoPDV.IFOOD, Valor = "0", ValoresAceitos = "0|1", Obrigatorio = true)]
        //public bool AtuaizarDisponibilidade { get; set; }
    }
}