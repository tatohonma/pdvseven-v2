using System;

namespace a7D.PDV.Integracao.ERPCake.Model
{
    public class Sales_Order_Parcel_Group : ModelCake
    {
        public int sales_order_id; // Identificador do pedido
        public Decimal price; // Valor pago na forma de pagamento
        public int payment_form; // Identificador <formas_pagamento>
        public Decimal total_discount; // Total de desconto da forma de pagamento
        public Decimal total_addition; // Total de acréscimo da forma de pagamento
        public Decimal change; // Troco da forma de pagamento

        public override string ToString() => $"{sales_order_id} R$ {price} R$ {total_discount}";
    }
}
