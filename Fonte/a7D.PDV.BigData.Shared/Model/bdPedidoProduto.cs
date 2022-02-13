using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace a7D.PDV.BigData.Shared.Model
{
    public class bdPedidoProduto
    {
        [Key, Column(Order = 2), Required]
        public int IDPedidoProduto { get; set; }

        public int IDPedido { get; set; }

        public int IDProduto { get; set; }

        public int? IDPedidoProduto_pai { get; set; }

        public decimal Quantidade { get; set; }

        public decimal ValorUnitario { get; set; }

        public DateTime DtInclusao { get; set; }
    }
}
