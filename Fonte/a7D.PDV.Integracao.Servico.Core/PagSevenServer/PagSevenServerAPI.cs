using a7D.PDV.Integracao.API2.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.Servico.Core.PagSevenServer
{
    public class PagSevenServerAPI
    {
        private HttpClient httpClient = new HttpClient();
        private Uri baseUrl;
        

        public PagSevenServerAPI()
        {
            httpClient.DefaultRequestHeaders.Clear();
            baseUrl = new Uri(ConfigurationManager.AppSettings["APIPagSeven"].ToString());
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("API_KEY", "oK8fEJkrETgEIXh+GXiT/4bO3FXFQSoQahSmUtn9cit1h/9QVL85Vw==");

        }

        public async Task<HttpResponseMessage> AlterarRespostaConsulta(Pedido pedido,int idConsulta)
        {
            var endereco = new Uri(baseUrl, ConfigurationManager.AppSettings["APIPagSevenAlterarRespostaConsulta"].ToString() + $"/{idConsulta}");
            var pedidoJson = JsonRespostaConsulta(pedido);
            var content = new StringContent(pedidoJson, Encoding.UTF8, "application/json");
            return await httpClient.PostAsync(endereco, content);
        }

        public async Task<HttpResponseMessage> GetConsultas(int idEstabelecimento, DateTime dataMinima)
        {
            var endereco = new Uri(baseUrl, ConfigurationManager.AppSettings["APIPagSevenGetConsultas"].ToString() + $"/{idEstabelecimento}?dataMinima={dataMinima.ToString("yyyy-MM-ddTHH:mm:ss")}&dataAgora={DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")}");         
            
            return await httpClient.GetAsync(endereco);
        }

        public async Task<HttpResponseMessage> GetPagamentosPendentes(int idEstabelecimento, DateTime dataMinima)
        {
            var endereco = new Uri(baseUrl, ConfigurationManager.AppSettings["APIPagSevenGetPagamentosPendentes"].ToString() + $"/{idEstabelecimento}?dataMinima={dataMinima.ToString("yyyy-MM-ddTHH:mm:ss")}&dataAgora={DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")}");

            return await httpClient.GetAsync(endereco);
        }

        public async Task<HttpResponseMessage> GetPagamentosAutorizados(int idEstabelecimento, DateTime dataMinima)
        {
            var endereco = new Uri(baseUrl, ConfigurationManager.AppSettings["APIPagSevenGetPagamentosAutorizados"].ToString() + $"/{idEstabelecimento}?dataMinima={dataMinima.ToString("yyyy-MM-ddTHH:mm:ss")}&dataAgora={DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")}");

            return await httpClient.GetAsync(endereco);
        }

        public async Task<HttpResponseMessage> AlterarStatusIntegracaoPagamento(Models.Pagamento pagamento)
        {
            var endereco = new Uri(baseUrl, ConfigurationManager.AppSettings["APIPagSeven_PagamentoAlterarStatusIntegracao"].ToString());
            var content = new StringContent(JsonConvert.SerializeObject(pagamento), Encoding.UTF8, "application/json");
            return await httpClient.PostAsync(endereco, content);
        }

        public HttpResponseMessage EnviarPedido(object modelPedidoPag7)
        {
            
            var endereco = new Uri(
                baseUrl,
                ConfigurationManager.AppSettings["APIPagSevenPedido"].ToString()
                );

            var content = new StringContent(
                JsonConvert.SerializeObject(modelPedidoPag7),
                Encoding.UTF8, "application/json");
            return httpClient.PostAsync(endereco, content).Result;

        }

        public string JsonRespostaConsulta(Pedido pedido)
        {
            return JsonConvert.SerializeObject(pedido);
        }

        public HttpResponseMessage FecharPedido(int idEstabelecimento, int idPedido)
        {
            
            var rota = ConfigurationManager.AppSettings["APIPagSevenFecharPedido"].ToString();
            rota = Regex.Replace(rota, "{idPedido}", idPedido.ToString());
            rota = Regex.Replace(rota, "{idEstabelecimento}", idEstabelecimento.ToString());
            var endereco = new Uri(baseUrl,rota);
            return httpClient.GetAsync(endereco).Result;
        }
    }
}
