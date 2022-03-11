using System;

namespace a7D.PDV.Integracao.iFood
{
    // https://developer.ifood.com.br/v1.0/reference#skusexternalcodeprices-1
    public class ProdutoPrecoIFood
    {
        public int[] merchantIds;
        public string externalCode;
        public decimal price;
        public DateTime startDate;
    }
}
