using System;

namespace a7D.PDV.BigData.Shared.ValueObject
{
    public class bdAlteracaoInfo
    {
        public DateTime? Produto { get; set; }

        public int? TipoPagamento { get; set; }

        public DateTime? Cliente { get; set; }

        public DateTime? Usuario { get; set; }

        public DateTime? Pedido { get; set; }

        public DateTime? UltimoSincronismo { get; set; }

        public string Mensagem { get; set; }

        public override string ToString()
        {
            return $"{Mensagem} {UltimoSincronismo}";
        }

    }
}
