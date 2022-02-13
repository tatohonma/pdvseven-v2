using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace a7D.PDV.BigData.Shared.Model
{
    public class bdProduto
    {
        [Key, Column(Order = 2), Required]
        public int IDProduto { get; set; }

        [Required]
        public string Nome { get; set; }

        public string EAN { get; set; }

        public decimal Valor { get; set; }

        public decimal Custo { get; set; }

        public bool Ativo { get; set; }

        public bool ControlarEstoque { get; set; }

        public decimal EstoqueMinimo { get; set; }

        public decimal EstoqueIdeal { get; set; }

        public decimal EstoqueAtual { get; set; }

        [Required]
        public DateTime dtAlteracao { get; set; }

    }
}
