using a7D.PDV.Integracao.Servico.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace a7D.PDV.Integracao.iFood.API
{
    public class Merchant : APIJson
    {
        public Merchant(string token) : base("https://merchant-api.ifood.com.br", token)
        {
        }

        public Model.Merchant.Status[] Status(string id) => Get<Model.Merchant.Status[]>($"merchant/v1.0/merchants/{id}/status");

        public Model.Merchant.Merchant Details(string id) => Get<Model.Merchant.Merchant>($"merchant/v1.0/merchants/{id}");

        public Model.Merchant.MerchantSummary[] List() => Get<Model.Merchant.MerchantSummary[]>($"merchant/v1.0/merchants");
    }
}
