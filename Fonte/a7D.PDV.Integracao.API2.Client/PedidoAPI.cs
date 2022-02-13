using a7D.PDV.Integracao.API2.Model;
using System.Collections.Generic;

namespace a7D.PDV.Integracao.API2.Client
{
    public class PedidoAPI
    {
        private ClienteWS api;

        internal PedidoAPI(ClienteWS ws)
            => api = ws;

        public ResultadoOuErro AdicionaPedido(AdicionarProdutos dado)
            => api.Post<ResultadoOuErro>("api/pedidos", dado);

        public ResultadoOuErro Fechar(FechamentoPedido fechamento)
            => api.Post<ResultadoOuErro>($"api/pedidos/fechar", fechamento);

        public Mesa MesaStatus(int numero, bool notFoundNull = false)
            => api.Get<Mesa>($"api/mesas/{numero}/total", notFoundNull);

        public Comanda ComandaStatus(object numero, bool notFoundNull = false)
            => api.Get<Comanda>($"api/comandas/{numero}/total", notFoundNull);

        public Comanda ComandaInfo(string numero, string tipo, bool notFoundNull = false)
            => api.Get<Comanda>($"api/comandas/{numero}/info/{tipo}", notFoundNull);

        public ResultadoOuErro ComandaRegistraCliente(string numero, string tipo, int idCliente, bool notFoundNull = false)
         => api.Post<ResultadoOuErro>($"api/comandas/{numero}/cliente/{tipo}", idCliente.ToString(), notFoundNull);

        /// <summary>
        /// Retorna as informações do total e mais o saldo (faz mais consultas)
        /// </summary>
        public Comanda ComandaSaldo(int numero, bool notFoundNull = false)
            => api.Get<Comanda>($"api/comandas/{numero}/saldo", notFoundNull);

        public Pedido ComandaItens(int numero, bool notFoundNull = false)
            => api.Get<Pedido>($"api/comandas/{numero}/itens", notFoundNull);

        public ExtratoItens[] ComandaExtrato(int numero, bool notFoundNull = false)
            => api.Get<ExtratoItens[]>($"api/comandas/{numero}/extrato", notFoundNull);

        public ResultadoOuErro ComandaAbrir(ComandaAbrir abrir, bool notFoundNull = false)
            => api.Post<ResultadoOuErro>($"api/comandas/abrir", abrir, notFoundNull);

        public Entrada ComandaEntrada(bool notFoundNull = false)
            => api.Get<Entrada>($"api/comandas/entrada", notFoundNull);

        public Pedido MesaItens(int numero, bool notFoundNull = false)
            => api.Get<Pedido>($"api/mesas/{numero}/itens", notFoundNull);

        public Pedido Pedido(int numero, bool notFoundNull = false)
            => api.Get<Pedido>($"api/pedidos/{numero}", notFoundNull);

        public ResultadoOuErro AdicionaPagamento(InsercaoPagamento dado)
            => api.Post<ResultadoOuErro>($"api/pagamentos", dado);

        public List<Pedido> GetNovosPedidos()
            => api.Get<List<Pedido>>("api/pedidos");
    }
}