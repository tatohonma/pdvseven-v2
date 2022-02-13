using System;

namespace a7D.PDV.Integracao.ERPCake.Model
{
    public class Sales_Order_Item : ModelCake
    {
        public int sales_order_id; // Identificar do pedido
        public int? seller_id; // Identificar do vendedor
        public int product_id; // Identificar do produto
        public int? grid; // Identificar da grade
        public int? line_feature; // Identificar do valor da característica de linha
        public int? column_feature; // Identificar do valor da característica de coluna
        public int? service_id; // Identificar do serviço
        public string item_name; // Nome do item
        public string item_add_info; // Informações adicionais do item
        public string item_type; // Identificador do tipo do item
        public int? item_number; // Número
        public Decimal qtd; // Quantidade
        public Decimal price_sell; // Preço de venda
        public Decimal price_cost; // Preço de custo
        public Decimal? discount_amount; // Total de desconto
        public string discount_type; // Tipo de desconto
        public Decimal? discount_percent; // Valor do percentual de desconto
        public Decimal? addition_amount; // Total de acréscimo
        public string addition_type; // Tipo de acréscimo
        public Decimal? addition_percent; // Valor do percentual de acréscimo
        public Decimal total; // Valor total
        public bool? canceled; // Flag que indica se está cancelado
        public Decimal? qtd_canceled; // Quantidade cancelada
        public Decimal? canceled_amount; // Valor de desconto cancelado
        public string round_type; // Tipo de arrendondamento
        public Decimal? discount_coupon_prorated; // Desconto rateado no cupom
        public Decimal? addition_coupon_prorated; // Acréscimo rateado no cupom
        public Decimal? price_taxable; // Valor tributavel
        public Decimal? shipping_coupon_prorated; // Transporte rateado
        public Decimal? expense_coupon_prorated; // Despesas rateadas

        public override string ToString() => $"{id}: {item_name}   {qtd} x R$ {price_sell}";
    }
}