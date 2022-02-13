using Newtonsoft.Json;
using System;

namespace a7D.PDV.Integracao.ERPCake.Model
{
    // http://app.cakeerp.com/api_docs/servicos.html#pedidos
    public class Sales_Order : ModelCake
    {
        public int order_number; // Número do pedido
        public int order_type = 1; // Tipo do pedido (Venda)
        public bool? environment_order; // Flag se é pedido por ambiente
        public int? ecf_number; // Número da impressora fiscal <impressoras_fiscais>
        public int? note_number; // Número da nota
        public string note_series; // Série da nota
        public int? ccf_number; // Contador do cupom fiscal
        public string delivery_time; // Tempo da compra
        public string date_order; // Data do pedido
        public string date_sell; // Data da venda
        public string date_billed; // Data do Faturamento
        public string emission_date; // Data da emissão
        public int? supllier; // Identificador do fornecedr
        public int? seller; // Identificador do vendedor
        public int customer; // Identificador do cliente
        public string resume; // 
        public int? price_list; // Identificador da lista de preços
        public Decimal? subtotal; // Subtotal do pedido
        public Decimal discount_amount; // Total de desconto
        public string discount_type; // Tipo de desconto
        public Decimal? discount_percent; // Valor percentual do desconto
        public Decimal? addition_amount; // Total de acréscimo
        public Decimal? addition_canceled; // Acréscimo cancelado
        public string addition_type; // Tipo de acréscimo
        public Decimal? addition_percent; // Valor percentual do acréscimo
        public Decimal total; // Valor total
        public string addition_discount_order; // Ordem de aplicação do desconto e acréscimo
        public int? payment_form; // Forma de pagamento
        public string obs; // Observações
        public int? validity; // Validade em dias
        public bool? shipping; // Flag que indica se tem transporte
        public int? shipping_transporter; // Identificador da transportadora
        public Decimal? shipping_price; // valor do transporte
        public int? invoice_model; // Modelo da nota: 55 nfe, 65 cupom, 59 sat
        public bool? canceled; // Flag que indica se esta cancelado
        public bool? inutilized; // Flag que indica se está inutilizado
        public int? fiscal_operation; // Identificador da operação fiscal
        public bool? nfe; // Flag que identifica se tem nota fiscal emitida
        public DateTime? nfe_date; // Data da nota fiscal
        public int? return_code; // Código de retorno da nota fiscal
        public string nfe_key; // Chave da nota fiscal
        public string nfe_status; // Status da nota fiscal
        public string nfe_status_msg; // Mensagem da nota fiscal
        public string nfe_xmotivo; // Motivo da nota fiscal
        public string nfe_nrec; // Número de retorno
        public string nfe_nprot; // Número do protocolo
        public string nfe_danfe; // Nome do arquivo da danfe
        public DateTime? pdv_ini_dth; // Data / Hora inicial do cupom no PDV
        public DateTime? pdv_end_dth; // Data / Hora final do cupom no PDV
        public Decimal? change_total; // Valor do troco
        public string customer_cpf_cnpj; // CPF ou CNPJ do cliente
        public int? ecf_printer; // Identificador da impressora fiscal
        public int? indPag; // Tipo de pagamento, usado somente para NFe
        public int? shipping_quantity; // Quntidade de volumes para transporte
        public Decimal? shipping_gross_weight; // Peso bruto de transporte
        public Decimal? shipping_net_weight; // Peso líquido para transporte
        public string shipping_species; // Tipo de transporte
        public string shipping_brand; // 
        public string shipping_numeration; // 
        public string nfe_key_code; // Còdigo da chave de NFe
        public int? nfe_key_dv; // 
        public bool? nfe_verified; // 
        public string nfe_id; // 
        public int? sales_return_id; // Identificador do pedido de devolução
        public string supplier_label; // Descrição do fornecedor
        public string external_sale_number; // Número externo da venda
        public int? sales_order_ref; // Venda Referenciada
        public bool? delivered_order; // Venda somente de entrega, venda futura com entrega posterior
        public int? ecommerce_id; // Identificador do ecommerce integrado
        public Decimal? other_expenses; // Outras despesas
        public string contact_name; // Nome de contato
        public string internal_obs; // Observações internas do pedido

        [JsonIgnore]
        public DateTime? Date_OrderConvert { get => GetDate(date_order); set => date_order = SetDate(value); }

        [JsonIgnore]
        public DateTime? Delivery_TimeConvert { get => GetDate(delivery_time); set => delivery_time = SetDate(value); }

        public override string ToString() => $"{id}: {order_number} R$ {total.ToString("N2")} (-R$ {discount_amount.ToString("N2")})";
    }
}