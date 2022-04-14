using a7D.PDV.Integracao.Servico.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace a7D.PDV.Integracao.iFood
{
    public class APIiFood : APIJson
    {
        public readonly string client_id = "pdvseven";
        public readonly string client_secret = "F_Z)f4kk";

        private string lojaID;
        public string Token { get; private set; }
        public DateTime TokenValidade { get; private set; }

        public APIiFood() : base("https://pos-api.ifood.com.br/")
        {
        }

        public string Autenticar(string merchant_id, string username, string password)
        {
            // https://developer.ifood.com.br/v1.0/reference#oauthtoken
            var uri = Query("oauth/token");
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", client_id), // e-POS id
                new KeyValuePair<string, string>("client_secret", client_secret), // e-POS password
                new KeyValuePair<string, string>("grant_type", "password"), // allways 'password'
                new KeyValuePair<string, string>("username", username), // merchant's username
                new KeyValuePair<string, string>("password", password) // merchant's password
            });

            var response = client.PostAsync(uri, formContent).Result;
            var resposta = response.Content.ReadAsStringAsync().Result;
            var token = JsonConvert.DeserializeObject<Token>(resposta);
            if (token.access_token == null)
                return token.error_description;

            client.DefaultRequestHeaders.Add("Authorization", $"{token.token_type} {token.access_token}");

            this.Token = token.access_token;
            this.TokenValidade = DateTime.Now.AddSeconds(token.expires_in); // Validade maxima do token

            lojaID = merchant_id;

            return "OK";
        }

        public Evento[] EventoPendentes() => Get<Evento[]>("v1.0/events%3Apolling");

        public string EventoLido(string cid) => EventosLidos(new EventoID[] { new EventoID() { id = cid } });

        public string EventosLidos(EventoID[] eventos) => Send<string>("v1.0/events/acknowledgment", eventos);

        // https://developer.ifood.com.br/v1.0/reference#ordersreference
        public PedidoIFood Pedido(long id) => Get<PedidoIFood>($"v1.0/orders/{id}");

        public string PedidoIntegrado(string id) => Send<string>($"v1.0/orders/{id}/statuses/integration");

        public string PedidoConfirmado(string id) => Send<string>($"v1.0/orders/{id}/statuses/confirmation");

        public string PedidoEnviado(string id) => Send<string>($"v1.0/orders/{id}/statuses/dispatch");

        public string PedidoEntregue(string id) => Send<string>($"v1.0/orders/{id}/statuses/delivery");

        public string PedidoRejeitado(string id, string motivo) => Send<string>($"v1.0/orders/{id}/statuses/rejection", new { details = motivo });

        public string StatusLoja(bool ativo, string razao)
        {
            var status = new LojaStatusIFood()
            {
                status = ativo ? "AVAILABLE" : "UNAVAILABLE",
                reason = razao
            };
            return Send<string>($"v1.0/merchants/{lojaID}/statuses", status, "PUT");
        }

        public string ProdutoPreco(ProdutoPrecoIFood prod) => Send<string>($"v1.0/skus/{prod.externalCode}/prices", prod, "PATCH");

        public string ProdutoDisponibilidade(string merchantId, string externalCode, bool ativo)
        {
            var status = new
            {
                status = ativo ? "AVAILABLE" : "UNAVAILABLE",
            };
            return Send<string>($"v1.0/merchants/{merchantId}/skus/{externalCode}", status, "PATCH");
        }
    }
}