using a7D.PDV.Integracao.Servico.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace a7D.PDV.Integracao.iFood.API
{
    public class OAuth : APIJson
    {
        public OAuth() : base("https://merchant-api.ifood.com.br")
        {
        }

        public Model.OAuth.Token Token(string grantType, string clientId, string clientSecret, string authorizationCode, string authorizationCodeVerifier, string refreshToken)
        {
            var uri = Query("/authentication/v1.0/oauth/token");
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grantType", grantType),
                new KeyValuePair<string, string>("clientId", clientId),
                new KeyValuePair<string, string>("clientSecret", clientSecret),
                new KeyValuePair<string, string>("authorizationCode", authorizationCode),
                new KeyValuePair<string, string>("authorizationCodeVerifier", authorizationCodeVerifier),
                new KeyValuePair<string, string>("refreshToken", refreshToken)
            });

            var response = client.PostAsync(uri, formContent).Result;
            var resposta = response.Content.ReadAsStringAsync().Result;
            var token = JsonConvert.DeserializeObject<Model.OAuth.Token>(resposta);

            return token;
        }
    }
}