namespace a7D.PDV.Integracao.API2.Model
{
    public partial class TipoPagamento
    {
        public TipoPagamento(int? IDTipoPagamento = default(int?),
            string Nome = default(string),
            string CodigoPagamentoSat = default(string))
        {
            this.IDTipoPagamento = IDTipoPagamento;
            this.Nome = Nome;
            this.CodigoPagamentoSat = CodigoPagamentoSat;
        }

        public TipoPagamento() { }

        public int? IDTipoPagamento { get; set; }
        public int? IDGateway { get; set; }
        public string Nome { get; set; }
        public string CodigoERP { get; set; }
        public MeioPagamento MeioPagamento { get; set; }
        /// <summary>
        /// Codigo de pagamento para o S@T 01: Dinheiro 02: Cheque 03: Cartão de Crédito 04: Cartão de Débito 05: Crédito Loja 10: Vale Alimentação 11: Vale Refeição 12: Vale Presente 13: Vale Combustível 99: Outros 
        /// </summary>
        /// <value>Codigo de pagamento para o S@T 01: Dinheiro 02: Cheque 03: Cartão de Crédito 04: Cartão de Débito 05: Crédito Loja 10: Vale Alimentação 11: Vale Refeição 12: Vale Presente 13: Vale Combustível 99: Outros </value>
        public string CodigoPagamentoSat { get; set; }
        public ContaRecebivel ContaRecebivel { get; set; }

    }

}
