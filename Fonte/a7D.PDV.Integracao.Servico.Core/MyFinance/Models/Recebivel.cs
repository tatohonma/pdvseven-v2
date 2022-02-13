using a7D.PDV.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

namespace a7D.PDV.Integracao.Servico.Core.MyFinance.Models
{
    public class Recebivel
    {
        public Recebivel()
        {
            CustomClassifications = new List<object>();
        }

        public Recebivel(API2.Model.Pedido pedido)
        {
            if (pedido.Pagamentos == null || pedido.Pagamentos.Count == 0)
                throw new Exception($"Erro na construção do recebíbel. Não há pagamentos para o pedido {pedido.IDPedido}");

            CustomClassifications = new List<object>();
            Pedido = pedido;
            PaymentMethod = ExtrairPaymentMethod(pedido);
            Issuer = ExtrairIssuer(pedido);
            SaleAccountID = ExtrairSaleAccountID(pedido);
        }



        [JsonProperty(PropertyName = "sale_account_id")]
        public int SaleAccountID { get; set; }
        [JsonProperty(PropertyName = "nominal_amount")]
        public decimal NominalAmount { get; set; }
        [JsonProperty(PropertyName = "occurred_at")]
        public DateTime OcurredAt { get; set; }
        [JsonProperty(PropertyName = "confirmed_at")]
        public DateTime ConfirmedAt { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "payment_method")]
        public string PaymentMethod { get; set; }
        [JsonProperty(PropertyName = "issuer")]
        public string Issuer { get; set; }
        [JsonProperty(PropertyName = "custom_classifications")]
        public List<Object> CustomClassifications { get; set; }

        public API2.Model.Pedido Pedido
        {
            get; set;
        }

        //CodigoERP = sale_account_id | issuer | payment_method
        public string ExtrairPaymentMethod(API2.Model.Pedido pedido)
        {
            if (pedido.Pagamentos.FirstOrDefault().IDMetodo == null)
            {
                var erro = new Exception($"O pagamento não possui método de pagamento. Pedido {pedido.IDPedido}; Pagamento {pedido.Pagamentos[0].ID}");
                erro.Data.Add("Pedido", JsonConvert.SerializeObject(pedido));
                throw erro;

            }
            return MyFinanceCore.TraduzirMeioMetodoPagamento(pedido.Pagamentos.FirstOrDefault().IDMetodo.Value).ToString();
        }
        public string ExtrairIssuer(API2.Model.Pedido pedido)
        {
            return BLL.Bandeira.CarregarPorNome(pedido.Pagamentos.FirstOrDefault().Bandeira)?.Nome;
        }
        public int ExtrairSaleAccountID(API2.Model.Pedido pedido)
        {

            var codigoIntegracao = BLL.ContaRecebivel.CarregarPorNome(pedido.Pagamentos[0].ContaRecebivel).CodigoIntegracao;
            if (string.IsNullOrEmpty(codigoIntegracao))
            {
                var erro = new Exception($"Código de integração inválido. Verificar a conta de recebível deste pagamento. Pedido {pedido.IDPedido}; Pagamento {pedido.Pagamentos[0].ID};");
                erro.Data.Add("Pedido", JsonConvert.SerializeObject(pedido));
                throw erro;
            }

            try
            {
                return Convert.ToInt32(codigoIntegracao);
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possivel converter o CodigoIntegração para um tipo inteiro.", ex);
            }

        }

        public string JsonPostRecebivel()
        {
            dynamic recebivelJson = new { sale = this };
            return JsonConvert.SerializeObject(recebivelJson);
        }
        
    }




}
