using System.ComponentModel.DataAnnotations;

namespace a7D.PDV.BigData.Shared.ValueObject
{
    public class bdEstoque
    {
        [Required]
        public int IDProduto { get; set; }

        [Required]
        public decimal EstoqueAtual { get; set; }
    }
}
