using System;

namespace a7D.PDV.EF.DTO
{
    public class PagamentoTEF
    {
        public int IDPedidoPagamento { get; set; }
        public int IDPedido {get; set;}
        public DateTime DataPagamento { get; set; }
        public string Autorizacao { get; set; }
        public decimal Valor { get; set; }

        public override string ToString()
        {
            return $"{IDPedido} {Autorizacao} {Valor}";
        }
    }
}
