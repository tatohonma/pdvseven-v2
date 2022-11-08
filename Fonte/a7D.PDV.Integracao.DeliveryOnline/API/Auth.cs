using a7D.PDV.Integracao.Servico.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.DeliveryOnline.API
{
    public class Auth : APIJson
    {
        public Auth() : base("http://delivery.pdvseven.com.br")
        {
        }

        public Model.Token.Token Token (string username, string password, string device_name)
        {
            var uri = Query("api/token");

            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
                new KeyValuePair<string, string>("device_name", device_name)
            });

            var response = client.PostAsync(uri, formContent).Result;
            var resposta = response.Content.ReadAsStringAsync().Result;

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return JsonConvert.DeserializeObject<Model.Token.Token>(resposta);
            }
            else
            {
                string erroRetorno = "";
                erroRetorno += response.StatusCode + " - " + response.ReasonPhrase + "\r\n";
                erroRetorno += JsonConvert.SerializeObject(response) + "\r\n";

                throw new Exception(erroRetorno);
            }
        }
    }
}
