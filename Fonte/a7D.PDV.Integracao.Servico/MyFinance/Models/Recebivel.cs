using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.Servico.MyFinance.Models
{
    public class Recebivel
    {
        public Recebivel()
        {
            CustomClassifications = new List<object>();
        }

        public Recebivel(WS2.Models.Pedido pedido)
        {
            CustomClassifications = new List<object>();
            Pedido = pedido;
            PaymentMethod = ExtrairPaymentMethod(pedido);
            Issuer = ExtrairIssuer(pedido);
            SaleAccountID = ExtrairSaleAccountID(pedido);
        }



        [JsonProperty(PropertyName = "sale_account_id")]
        public int? SaleAccountID { get; set; }
        [JsonProperty(PropertyName = "nominal_amount")]
        public decimal NominalAmount { get; set; }
        [JsonProperty(PropertyName = "occurred_at")]
        public DateTime OcurredAt { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "payment_method")]
        public string PaymentMethod { get; set; }
        [JsonProperty(PropertyName = "issuer")]
        public string Issuer { get; set; }
        [JsonProperty(PropertyName = "custom_classifications")]
        public List<Object> CustomClassifications { get; set; }

        public WS2.Models.Pedido Pedido
        {
            get; set;
        }

        //CodigoERP = sale_account_id | issuer | payment_method
        public string ExtrairPaymentMethod(WS2.Models.Pedido pedido)
        {
            string paymentMethod = "";
            paymentMethod = pedido?.Pagamentos?[0].TipoPagamento.CodigoERP;

            if (!string.IsNullOrEmpty(paymentMethod))
            {
                paymentMethod = paymentMethod.Split('|')[2];
            }

            return paymentMethod;
        }
        public string ExtrairIssuer(WS2.Models.Pedido pedido)
        {
            string issuer = "";
            issuer = pedido?.Pagamentos?[0].TipoPagamento.CodigoERP;

            if (!string.IsNullOrEmpty(issuer))
            {
                issuer = issuer.Split('|')[1];
            }

            return issuer;
        }
        public int? ExtrairSaleAccountID(WS2.Models.Pedido pedido)
        {
            int? saleAcountId = null;
            string stringID;
            stringID = pedido?.Pagamentos?[0].TipoPagamento.CodigoERP;


            if (!string.IsNullOrEmpty(stringID))
            {
                saleAcountId = Convert.ToInt32(stringID.Split('|')[0]);
            }

            return saleAcountId;
        }

    }



    /*{
      "sale":
        {
          "sale_account_id": 1,
          "nominal_amount": 139.90,
          "occurred_at": "2017-04-24",
          "description":"Camiseta Listrada",
          "payment_method": "credit",
          "issuer": "MasterCard",
          "custom_classifications": [
            {
              "custom_classifier_id": 1,
              "value": "Norte"
            }]
        }
    }*/
}
