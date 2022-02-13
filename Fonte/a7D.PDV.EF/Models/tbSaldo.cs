using System;

namespace a7D.PDV.EF.Models
{
    public class tbSaldo
    {
        public int IDSaldo { get; set; }

        public DateTime dtMovimento { get; set; }

        public decimal Valor { get; set; }

        public string Tipo { get; set; }

        public int IDPai { get; set; } 

        public int IDPedido { get; set; }

        public int? IDPedidoPagamento { get; set; }

        public int IDCliente { get; set; }

        public string CodigoERP { get; set; } // Para rastrear no cake

        public DateTime? Liquidado { get; set; } // Data de baixa total, ou expiração

        public virtual tbPedido Pedido { get; set; }

        public virtual tbPedidoPagamento Pagamento { get; set; }

        public virtual tbCliente Cliente { get; set; }

        public override string ToString() => $"{IDSaldo}: {dtMovimento} Cliente: {IDCliente} Pedido: {IDPedido} {Tipo} R$ {Valor}";
    }
}
