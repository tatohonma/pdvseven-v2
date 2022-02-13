using a7D.PDV.EF.Enum;
using a7D.PDV.Integracao.API2.Model;
using a7D.PDV.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.Servico.Core.PdvSeven
{
    public class PDVSevenAPI
    {
        private HttpClient httpClient = new HttpClient();
        private Uri baseUrl;


        public PDVSevenAPI()
        {
            httpClient.DefaultRequestHeaders.Clear();
            baseUrl = new Uri(ConfigurationManager.AppSettings["EnderecoAPI2"].ToString());
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }     

        public async Task<HttpResponseMessage> GetPedido(int idPedido)
        {
            var endereco = new Uri(baseUrl, ConfigurationManager.AppSettings["API2_PedidoPorID"].ToString()
                + $"/{idPedido}");

         
            return await httpClient.GetAsync(endereco);
        }

        //public async Task<HttpResponseMessage> AlterarComandaParaStatusContaSolicitada(int idPedido)
        //{
        //    var rota = ConfigurationManager.AppSettings["API2_AlterarStatusComanda_ContaSolicitada"].ToString();
        //    rota = Regex.Replace(rota, "{idPedido}", idPedido.ToString());

        //    var endereco = new Uri(baseUrl,rota);
        //    return await httpClient.PostAsync(endereco,null);
        //}

        public async Task<HttpResponseMessage> AddPagamento(InsercaoPagamento pagamento)
        {
            var endereco = new Uri(baseUrl, ConfigurationManager.AppSettings["API2_Pagamento"].ToString());
            var content = new StringContent(JsonConvert.SerializeObject(pagamento), Encoding.UTF8, "application/json");
            return await httpClient.PostAsync(endereco, content);
        }

        public async Task<HttpResponseMessage> FecharPedido(string idPedido, FechamentoPedido fechamento)
        {
            var rota = ConfigurationManager.AppSettings["API2_FecharPedido"].ToString();
            rota = Regex.Replace(rota, "{idPedido}", idPedido);
            
            var endereco = new Uri(baseUrl, rota);
            var content = new StringContent(JsonConvert.SerializeObject(fechamento), Encoding.UTF8, "application/json");
            return await httpClient.PostAsync(endereco, content);
        }
        public HttpResponseMessage GetContasRecebiveis()
        {
            var endereco = new Uri(baseUrl, ConfigurationManager.AppSettings["API2_ContaRecebivel"].ToString()); 
            return httpClient.GetAsync(endereco).Result;
        }

        public HttpResponseMessage GetTiposPagamento()
        {
            var endereco = new Uri(baseUrl, ConfigurationManager.AppSettings["API2_Pagamento"].ToString());
            return httpClient.GetAsync(endereco).Result;
        }

        public HttpResponseMessage PutCodigoIntegracaoContaRecebivel(API2.Model.ContaRecebivel contaRecebivel)
        {            
            var endereco = new Uri(baseUrl, ConfigurationManager.AppSettings["API2_ContaRecebivelPutCodigoIntegracao"].ToString());
            var content = new StringContent(JsonConvert.SerializeObject(contaRecebivel), Encoding.UTF8, "application/json");
            return httpClient.PutAsync(endereco, content).Result;
        }

        public HttpResponseMessage GetPedidosAbertos(int segundosAtras, ETipoPedido tipoPedido)
        {
            var endereco = new Uri(baseUrl, $"/api/pedidos/abertos?segundosAtras={segundosAtras}&idTipoPedido={(int)tipoPedido}");
            return httpClient.GetAsync(endereco).Result;
        }
        public HttpResponseMessage GetPedidosFechados(long dtInicio, long dtFim, int pagina, int qtRegistros)
        {

            var endereco = new Uri(baseUrl, "api/pedidos");
            endereco = endereco
                .AddQuery("dataInicio", $"{dtInicio}")
                .AddQuery("dataFim", $"{dtFim}")
                .AddQuery("pagina", $"{pagina}")
                .AddQuery("qtRegistros", $"{qtRegistros}");

            return httpClient.GetAsync(endereco).Result;


        }
    }
}
