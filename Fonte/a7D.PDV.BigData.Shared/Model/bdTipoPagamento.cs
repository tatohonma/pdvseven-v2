using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace a7D.PDV.BigData.Shared.Model
{
    public class bdTipoPagamento
    {
        [Key, Column(Order = 2), Required]
        public int IDTipoPagamento { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public DateTime dtAlteracao { get; set; }
    }
}
