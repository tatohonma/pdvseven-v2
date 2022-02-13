using a7D.PDV.EF;

namespace a7D.PDV.BLL
{
    public class ConfiguracoesServicoIntegracoes : ConfiguracaoBD
    {
        public ConfiguracoesServicoIntegracoes() : base(null, null)
        {
        }

        [Config("Intervalo que o serviço de integrações faz a sincronização com a API MyFinance (em segundos)", Valor = "3600")]
        public string intervaloSyncMyFinance { get; protected set; }

        [Config("Token de acesso à API do MyFinance")]
        public string authTokenMyFinance { get; set; }

        [Config("Data de fechamento do último pedido enviado ao MyFinance", Valor = null)]
        public string dtUltimaFechamentoPedidoMyFinance { get; set; }

        [Config("ID do último pedido enviado ao MyFinance", Valor = null)]
        public string ultimoIDPedidoMyFinance { get; set; }

        [Config("ID do último pagamento enviado ao MyFinance", Valor = null)]
        public string ultimoIDPagamentoMyFinance { get; set; }

        [Config("Flag que indica se o último pagamento foi enviado com sucesso ao MyFinance", Valor = "0", ValoresAceitos = "0|1")]
        public string EnviadoMyFinance { get; set; }

        [Config("Habilita integração MyFinance", Valor = "0", ValoresAceitos = "0|1")]
        public string HabilitaMyFinance { get; set; }

        [Config("Habilita PagSeven", Valor = "")]
        public string HabilitaPagSeven { get; set; }

        [Config("Última Consulta no PagSeven", Valor = "")]
        public string dtUltimaConsultaPagSeven { get; set; }

        [Config("Intervalo de Consulta no PagSeven", Valor = "")]
        public string IntervaloSyncConsultasPagSeven { get; set; }

        [Config("Data último fechamento de pedido no PagSeven", Valor = "")]
        public string dtFechamentoUltimoPedidoFechadoPag7 { get; set; }
    }
}
