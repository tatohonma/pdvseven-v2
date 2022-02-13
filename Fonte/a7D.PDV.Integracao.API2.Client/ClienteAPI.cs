using a7D.PDV.Integracao.API2.Model;
using System.Collections.Generic;

namespace a7D.PDV.Integracao.API2.Client
{
    public class ClienteAPI
    {
        private ClienteWS api;

        internal ClienteAPI(ClienteWS ws)
            => api = ws;

        public List<Cliente> ListaCliente(string nome = null, string email = null, string documento = null, string telefone = null, int pagina = 1, int qtd = 10)
            => api.Get<List<Cliente>>($"api/clientes?nome={nome}&email={email}&documento={documento}&telefone={telefone}&pagina={pagina}&qtd={qtd}");

        public Cliente Carregar(int id)
            => api.Get<Cliente>($"api/clientes/{id}");

        public ResultadoOuErro SaldoCliente(int id)
            => api.Get<ResultadoOuErro>($"api/clientes/{id}/saldo");

        public ResultadoOuErro InserirCliente(Cliente cliente)
            => api.Post<ResultadoOuErro>($"api/clientes/inserir", cliente);
    }
}