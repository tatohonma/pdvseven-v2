using a7D.PDV.EF.Enum;
using a7D.PDV.Integracao.API2.Model;
using a7D.PDV.Integracao.Servico.Core.PdvSeven;
using a7D.PDV.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.Servico.Core.PDVSeven
{
    public class PDVSevenCore
    {
        HttpClient httpClient;
        Uri baseUrl;
        public PDVSevenCore()
        {
            httpClient = new HttpClient();
            baseUrl = new Uri(ConfigurationManager.AppSettings["EnderecoAPI2"].ToString());
        }

        public async Task<List<API2.Model.Pedido>> ObterVendasAPI2Async(long dtInicio, long dtFim, int qtRegistros)
        {
            List<API2.Model.Pedido> listaPedidos = new List<API2.Model.Pedido>();
            int pagina = 1;
            int countTotalPesquisa = 0;
            var response = new PDVSevenAPI().GetPedidosFechados(dtInicio, dtFim, pagina, qtRegistros);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Erro na conexão com PDV7: {response.StatusCode} {response.ReasonPhrase}");
            }
            var headerTotalPesquisa = response.Headers.GetValues("countTotalPesquisa").FirstOrDefault();
            var pedidosString = await response.Content.ReadAsStringAsync();
            listaPedidos.AddRange(JsonConvert.DeserializeObject<List<API2.Model.Pedido>>(pedidosString));

            if (!string.IsNullOrEmpty(headerTotalPesquisa))
            {
                countTotalPesquisa = Convert.ToInt32(headerTotalPesquisa);

                while (qtRegistros * pagina < countTotalPesquisa)
                {
                    pagina++;
                    response = new PDVSevenAPI().GetPedidosFechados(dtInicio, dtFim, pagina, qtRegistros);
                    var responseString = await response.Content.ReadAsStringAsync();
                    listaPedidos.AddRange(JsonConvert.DeserializeObject<List<API2.Model.Pedido>>(responseString));
                }
            }

            return listaPedidos;
        }

        public async Task<List<Produto>> ObterProdutosAPI2Async(long dtUltimaAlteracao)
        {
            var listaProdutos = new List<Produto>();
            var response = await GetProdutosAlterados(dtUltimaAlteracao);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Erro na conexão com PDV7: {response.StatusCode} {response.ReasonPhrase}");
            }

            var pedidosString = await response.Content.ReadAsStringAsync();
            listaProdutos.AddRange(JsonConvert.DeserializeObject<List<Produto>>(pedidosString));
            return listaProdutos;
        }
       
        public async Task<HttpResponseMessage> GetProdutosAlterados(long dtUltimaAlteracao)
        {
            try
            {
                var endereco = new Uri(baseUrl, "produtos");
                endereco = endereco
                    .AddQuery("data", $"{dtUltimaAlteracao}");

                return await httpClient.GetAsync(endereco);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ContaRecebivel> ListarContasRecebivel()
        {
            var api = new PDVSevenAPI();
            var result = api.GetContasRecebiveis();
            var resultBody = result.Content.ReadAsStringAsync().Result;

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception($@"Erro ao listar contas de Recebiveis da PDV7.
Codigo do erro: {(int)result.StatusCode} - {result.ReasonPhrase} - {resultBody}
");
            }
            return JsonConvert.DeserializeObject<List<ContaRecebivel>>(resultBody);

        }       

        public static List<TipoPagamento> ListarTiposPagamento()
        {
            var api = new PDVSevenAPI();
            var result = api.GetTiposPagamento();
            var resultBody = result.Content.ReadAsStringAsync().Result;

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception($@"Erro ao listar tipos de pagamento da PDV7.
Codigo do erro: {(int)result.StatusCode} - {result.ReasonPhrase} - {resultBody}
");
            }
            return JsonConvert.DeserializeObject<List<TipoPagamento>>(resultBody);
        }

        public static void AlterarCodigoIntegracaoContaRecebivel(ContaRecebivel contaRecebivel)
        {
            var api = new PDVSevenAPI();
            var response = api.PutCodigoIntegracaoContaRecebivel(contaRecebivel);
            
            if (!response.IsSuccessStatusCode)
            {
                var responseBody = response.Content.ReadAsStringAsync().Result;
                throw new Exception($@"Erro ao atualizar codigo de integração: {response.StatusCode} {response.ReasonPhrase}
Recebível: {JsonConvert.SerializeObject(contaRecebivel)}
Erro: {responseBody.ToString()}");
            }
           
        }

        public static List<Pedido> PedidosAbertos(int segundosAtras, ETipoPedido tipoPedido)
        {
            var resposta = new PDVSevenAPI().GetPedidosAbertos(segundosAtras,tipoPedido);
            var responseBody = resposta.Content.ReadAsStringAsync().Result;
            if (!resposta.IsSuccessStatusCode)
            {
                throw new Exception($@"Erro ao listar pedidos abertos na PDV7.
Codigo do erro: {(int)resposta.StatusCode} - {resposta.ReasonPhrase} - {responseBody}
");
            }
            return JsonConvert.DeserializeObject<List<Pedido>>(responseBody);
        }
    }
}
