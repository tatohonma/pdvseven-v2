using a7D.PDV.Integracao.ERPSige.Model;
using a7D.PDV.Integracao.Servico.Core;

namespace a7D.PDV.Integracao.ERPSige
{
    public class APIERP : APIJson
    {
        public string Token { get; private set; }

        public APIERP() : base("https://erp.pdvseven.com.br/api/request/")
        {
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization-Token", "2298b11cdc351b0c9235e1263e4c380e6be376bd34be05634a1de93574ba34d46dc9a3a83796954a5a8634fc68b4ed664f15e61dfe6c1c63e1533c712b7f3741fa823b2ce1f482061525ac3732de3b8caeb431de1887886e370a96e04eaf221694d7f9c5835eb3a7f8a49b7326072904344b291067cec2ce77889d332822ddeb");
            client.DefaultRequestHeaders.Add("User", "fabio@pdvseven.com.br");
            client.DefaultRequestHeaders.Add("App", "API");
        }

        #region Pessoa

        public Pessoa[] PessoaListar(int skip = 0) => Get<Pessoa[]>($"pessoas/getall?skip={skip}");

        public string PessoaSalvar(Pessoa pessoa) => Send<string>($"pessoas/salvar", pessoa);

        #endregion

        #region Pessoa

        public Produto[] ProdutoListar(int skip = 0) => Get<Produto[]>($"produtos/getall?skip={skip}");

        public Produto ProdutoCarregar(int codigo) => Get<Produto>($"produtos/get?codigo={codigo}");

        public string ProdutoSalvar(Produto produto) => Send<string>($"produtos/salvar", produto);

        #endregion

        #region Pedido

        public Pedido[] PedidoListar(int page = 0) => Get<Pedido[]>($"pedidos/gettodospedidos?page={page}");

        public string PedidoSalvar(Pedido pedido) => Send<string>($"pedidos/salvar", pedido);

        public string PedidoSalvarEFaturar(Pedido pedido) => Send<string>($"pedidos/salvarefaturar", pedido);

        #endregion
    }
}