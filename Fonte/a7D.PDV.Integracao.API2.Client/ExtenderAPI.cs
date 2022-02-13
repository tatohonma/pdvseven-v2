namespace a7D.PDV.Integracao.API2.Client
{
    public static class ExtenderAPI
    {
        public static ConfiguracaoAPI Configuracao(this ClienteWS ws)
            => new ConfiguracaoAPI(ws);

        public static AutenticacaoAPI Autenticacao(this ClienteWS ws)
            => new AutenticacaoAPI(ws);

        public static PedidoAPI Pedido(this ClienteWS ws)
            => new PedidoAPI(ws);

        public static ProdutoAPI Produto(this ClienteWS ws)
            => new ProdutoAPI(ws);

        public static ImpressaoAPI Impressao(this ClienteWS ws)
            => new ImpressaoAPI(ws);

        public static PainelMesasAPI PainelMesas(this ClienteWS ws)
            => new PainelMesasAPI(ws);

        public static TemaAPI Tema(this ClienteWS ws)
            => new TemaAPI(ws);

        public static ClienteAPI Cliente(this ClienteWS ws)
            => new ClienteAPI(ws);
    }
}
