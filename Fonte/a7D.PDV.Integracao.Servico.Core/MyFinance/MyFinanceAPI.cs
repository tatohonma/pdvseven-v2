using a7D.PDV.Integracao.Servico.Core.MyFinance.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.Servico.Core.MyFinance
{
    public class MyFinanceAPI
    {
        private HttpClient httpClient;

        public static Uri baseUrl;
        public static string authToken;
        public static string password;
        public static int idEntidade;

        public MyFinanceAPI()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();

            String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(authToken + ":" + password));
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + encoded);
        }


        public async Task<HttpResponseMessage> PostRecebivel(Recebivel recebivel)
        {
            var endereco = new Uri(baseUrl, $"entities/{idEntidade}/sales");
            var content = new StringContent(recebivel.JsonPostRecebivel(), Encoding.UTF8, "application/json");
            return await httpClient.PostAsync(endereco, content);
        }

        public HttpResponseMessage PostContaRecebivel(ContaRecebivel contaRecebivel)
        {
            var endereco = new Uri(baseUrl, $"/sale_accounts");
            var content = new StringContent(contaRecebivel.JsonPostContaRecebivel(), Encoding.UTF8, "application/json");
            return httpClient.PostAsync(endereco, content).Result;
        }

        public HttpResponseMessage ListaContasRecebivel()
        {
            var endereco = new Uri(baseUrl, $"/sale_accounts");
            return httpClient.GetAsync(endereco).Result;
        }

        public HttpResponseMessage ObterEntidade()
        {
            var endereco = new Uri(baseUrl, $"/entities");
            return httpClient.GetAsync(endereco).Result;
        }

        public HttpResponseMessage ConsultarContaRecebivel(string id)
        {
            var endereco = new Uri(baseUrl, $"/sale_accounts/{id}");
            return httpClient.GetAsync(endereco).Result;
        }

        public HttpResponseMessage AlterarContaRecebivel(ContaRecebivel contaRecebivel)
        {
            var endereco = new Uri(baseUrl, $"/sale_accounts/{contaRecebivel.ID}");
            var content = new StringContent(contaRecebivel.JsonPostContaRecebivel(), Encoding.UTF8, "application/json");
            return httpClient.PutAsync(endereco, content).Result;
        }

        public void DeletarRecebiveis(int idInicio, int idFim)
        {
            for (int i = idInicio; i <= idFim; i++)
            {
                var endereco = new Uri(baseUrl, $"entities/{idEntidade}/sales/{i}");
                var response = httpClient.DeleteAsync(endereco).Result;
            }
        }

        private void GetEntity()
        {
            var endereco = new Uri(baseUrl, $"entities");
            var response = httpClient.GetAsync(endereco).Result;
        }

        private void CriarCategoria()
        {
            var endereco = new Uri(baseUrl, "categories");
            var content = new StringContent("{\"category\":{\"name\":\"Categoria Tal\"}}", Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync(endereco, content).Result;
        }
    }
}


/* {
    "entity": {
        "account_id": 26465,
        "charging_uuid": null,
        "created_at": "2017-06-26T14:58:47-03:00",
        "default_in_menu": true,
        "deleted_at": null,
        "federation_subscription_number": null,
        "force_destroy": false,
        "id": 29737,
        "imported_from_sync_at": null,
        "name": "PDVSeven :: Sistemas para Bares e Restaurantes",
        "updated_at": "2017-06-26T14:58:47-03:00"
    }
} 
 */
