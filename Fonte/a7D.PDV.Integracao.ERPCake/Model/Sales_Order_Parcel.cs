using Newtonsoft.Json;
using System;

namespace a7D.PDV.Integracao.ERPCake.Model
{
    // http://app.cakeerp.com/api_docs/servicos.html#parcelas-do-pedido
    public class Sales_Order_Parcel : ModelCake
    {
        public int sales_order_id; // Identificado do pedido
        public int parcel; // Número da parcela
        public string expiration; // Data da expiração
        public Decimal price; // Valor da parcela
        public int payment_form; // Identificador Forma de pagamento
        public int sales_parcel_group; // Identificador Grupo de parcelas

        [JsonIgnore]
        public DateTime? ExpirationConvert { get => GetDate(expiration); set => expiration = SetDate(value); }

        public override string ToString() => $"{id}: {payment_form} R$ {price.ToString("N2")}";
    }
}