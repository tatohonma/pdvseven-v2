using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace a7D.PDV.BigData.Shared.Model
{
    public class bdPedidoPagamento
    {
        [Key, Column(Order = 2), Required]
        public int IDPedidoPagamento { get; set; }

        [Required]
        public int IDPedido { get; set; }

        [Required]
        public int IDTipoPagamento { get; set; }

        [Required]
        public decimal Valor { get; set; }
    }
}
