using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace a7D.PDV.BigData.Shared.Model
{
    public class bdPedido
    {
        public bdPedido()
        {
            Produtos = new List<bdPedidoProduto>();
            Pagamentos = new List<bdPedidoPagamento>();
        }

        [Key, Column(Order = 2), Required]
        public int IDPedido { get; set; }

        public int? IDCliente { get; set; }

        public DateTime DtPedidoFechamento { get; set; }

        public decimal ValorTotal { get; set; }

        public decimal ValorDesconto { get; set; }

        public decimal ValorEntrega { get; set; }

        public virtual IEnumerable<bdPedidoProduto> Produtos { get; set; }

        public virtual IEnumerable<bdPedidoPagamento> Pagamentos { get; set; }

        public override string ToString()
        {
            return $"{IDPedido}: {DtPedidoFechamento} R$ {ValorTotal}";
        }
    }
}
