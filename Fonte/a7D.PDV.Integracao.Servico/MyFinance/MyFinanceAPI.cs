using a7D.PDV.Integracao.Servico.MyFinance.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.Servico.MyFinance
{
    public class MyFinanceAPI
    {
        private static HttpClient httpClient = new HttpClient();
        private static Uri baseUrl = new Uri("https://app.myfinance.com.br");
        private static string authToken = "c5406fcd5d83a0a57f988a038142a86622aa83855759d051";
        private static string password = "";
        private static string idEntidadePDV7 = "29737";

        private static void InitClient()
        {
            httpClient.DefaultRequestHeaders.Clear();

            String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(authToken + ":" + password));
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + encoded);
        }
        

        public static async Task<HttpResponseMessage> PostRecebivel(Recebivel recebivel)
        {
            InitClient();
            var endereco = new Uri(baseUrl, $"entities/{idEntidadePDV7}/sales");
            var recebivelJson = JsonPostRecebivel(recebivel);
            var content = new StringContent(recebivelJson, Encoding.UTF8, "application/json");
            return await httpClient.PostAsync(endereco,content);
        }

        public static string JsonPostRecebivel(Recebivel recebivel)
        {
            dynamic recebivelJson = new { sale = recebivel };
            return JsonConvert.SerializeObject(recebivelJson);
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
