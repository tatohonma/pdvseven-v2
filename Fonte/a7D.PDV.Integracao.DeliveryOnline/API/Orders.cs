using a7D.PDV.Integracao.Servico.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace a7D.PDV.Integracao.DeliveryOnline.API
{
    public class Orders : APIJson
    {
        public Orders(string token) : base("http://delivery.pdvseven.com.br", token)
        {
        }

        public Model.Orders.OrdersInformation GetOrders(int locationId, int statusId) => Get<Model.Orders.OrdersInformation>($"/api/orders?location={locationId}&status={statusId}&include=address");

        public void UpdateStatus(string id, int status_id)
        {
            var uri = Query($"api/orders/{id}");

            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("status_id", status_id.ToString()),
            });

            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, uri)
            {
                Content = formContent
            };

            var response = client.SendAsync(request).Result;
            var resposta = response.Content.ReadAsStringAsync().Result;

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                string erroRetorno = "";
                erroRetorno += response.StatusCode + " - " + response.ReasonPhrase + "\r\n";
                erroRetorno += JsonConvert.SerializeObject(response) + "\r\n";

                throw new Exception(erroRetorno);
            }
        }
    }
}
