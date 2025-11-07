namespace a7D.PDV.Integracao.Pagamento.StoneTEF.Dto
{
    internal sealed class PayResponse
    {
        public string brandName { get; set; }
        public Receipt receipt { get; set; }
        public int transactionType { get; set; } // 1=Debit, 2=Credit, 7=Voucher
        public Card card { get; set; }
        public string authorisationCode { get; set; }
        public string messageDisplay { get; set; }

        internal sealed class Receipt
        {
            public string acquirerTransactionKey { get; set; } // ATK
            public decimal amount { get; set; }
            public string clientVia { get; set; }
            public string merchantVia { get; set; }
        }
        internal sealed class Card
        {
            public string maskedPrimaryAccountNumber { get; set; } // panMask
        }
    }
}
