using a7D.PDV.Integracao.ERPCake.Model;
using a7D.PDV.Model;

namespace a7D.PDV.Integracao.ERPCake.DTO
{
    public static class DTOconvert
    {
        public static Sales_Order Convert(this PedidoInformation pedidoPDV)
        {
            // http://app.cakeerp.com/api_docs/servicos.html#pedidos
            return new Sales_Order()
            {
                order_number = pedidoPDV.IDPedido.Value,
                order_type = 1, // FIXO: Venda
                Date_OrderConvert = pedidoPDV.DtPedido, // É sempre a data do PEDIDO ABERTO, e não o do fechamento para minimizar problemas, que sempre haverão
                Delivery_TimeConvert = pedidoPDV.DtPedidoFechamento,
                total = pedidoPDV.ValorTotal.Value,
                discount_amount = pedidoPDV.ValorDesconto ?? 0
            };
        }

        public static Sales_Order_Item Convert(this PedidoProdutoInformation pp)
        {
            return new Sales_Order_Item()
            {
                product_id = int.Parse(pp.Produto.CodigoERP), // A existencia do CódigoERP precisa ser validada previamente
                item_type = "product",
                item_name = pp.Produto.Nome,
                qtd = pp.Quantidade.Value,
                price_sell = pp.ValorUnitario.Value,
                total = pp.Quantidade.Value * pp.ValorUnitario.Value,
            };
        }

        public static Sales_Order_Parcel Convert(this PedidoPagamentoInformation pp, int pagFormID, int condParcelDays)
        {
            return new Sales_Order_Parcel()
            {
                parcel = 1,
                ExpirationConvert = pp.DataPagamento.Value.AddDays(condParcelDays),
                price = pp.Valor.Value,
                payment_form = pagFormID,
            };
        }

        public static Sales_Order_Parcel_Group ConvertP1(this PedidoPagamentoInformation pp, int pagFormID, decimal percTaxa)
        {
            return new Sales_Order_Parcel_Group()
            {
                price = pp.Valor.Value,
                payment_form = pagFormID,
                total_discount = pp.Valor.Value * percTaxa / 100m
            };
        }
    }
}
