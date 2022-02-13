using Newtonsoft.Json;
using System;

/*
http://app.myfinance.com.br/entities
[{
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
}]
*/

namespace a7D.PDV.Integracao.Servico.Core.MyFinance.Models
{
    public class Entidade
    {
        [JsonProperty(PropertyName = "entity")]
        public EntidadeItem entidade;
    }

    public class EntidadeItem
    {
        [JsonProperty(PropertyName = "account_id")]
        public int ContaMyFinance { get; set; }

        [JsonProperty(PropertyName = "created_at")]
        public DateTime DataCriacao { get; set; }

        [JsonProperty(PropertyName = "id")]
        public int EntidadeID { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Nome { get; set; }
    }
}
