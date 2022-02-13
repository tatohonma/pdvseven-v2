using a7D.PDV.Integracao.API2.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.Servico.Core.WorkinHub
{
    public class WorkinHubAPI
    {
        private HttpClient httpClient = new HttpClient();
        private Uri baseUrl;

        /// O parametro idLocal é o identificador único para cada cliente do WorkInHub. Obter este id com a equipe do WorkInHub 

        public WorkinHubAPI()
        {
            httpClient.DefaultRequestHeaders.Clear();
            baseUrl = new Uri(ConfigurationManager.AppSettings["APIWorkinHub"].ToString());
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<HttpResponseMessage> PostPedidos(List<Pedido> pedidos)
        {
            var endereco = new Uri(baseUrl, ConfigurationManager.AppSettings["APIWorkinHubEnviarProdutos"].ToString());
            var pedidoJson = JsonPostPedido(pedidos);
            var content = new StringContent(pedidoJson, Encoding.UTF8, "application/json");
            return await httpClient.PostAsync(endereco, content);
        }

        public async Task<HttpResponseMessage> PostProdutos(List<Produto> produtos)
        {
            var endereco = new Uri(baseUrl, ConfigurationManager.AppSettings["APIWorkinHubEnviarVendas"].ToString());
            var pedidoJson = JsonPostProduto(produtos);
            var content = new StringContent(pedidoJson, Encoding.UTF8, "application/json");
            return await httpClient.PostAsync(endereco, content);
        }

        public string JsonPostPedido(List<Pedido> pedido)
        {
            var idLocal = ServicoCore.Configuracoes().IdentificadorClienteWorkinHub;
            var listaParaWorkinHub = pedido.Select(p =>
            {
                var obj = ConvertHelper.ToDynamic(p);
                obj.idLocal = idLocal;
                return obj;
            }).ToList();

            return JsonConvert.SerializeObject(listaParaWorkinHub);
        }

        public string JsonPostProduto(List<Produto> produtos)
        {
            var coworkingID = ServicoCore.Configuracoes().IdentificadorClienteWorkinHub;
            var listaParaWorkinHub = produtos.Select(p =>
            {
                var obj = ConvertHelper.ToDynamic(p);
                obj.CoworkingID = coworkingID;
                return obj;
            }).ToList();

            return JsonConvert.SerializeObject(listaParaWorkinHub);
        }
    }
}
