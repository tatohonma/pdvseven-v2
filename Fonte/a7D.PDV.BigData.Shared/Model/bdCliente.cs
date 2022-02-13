using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace a7D.PDV.BigData.Shared.Model
{
    public class bdCliente
    {
        [Key, Column(Order = 2), Required]
        public int IDCliente { get; set; }

        [Required]

        public string NomeCompleto { get; set; }

        public DateTime? DataNascimento { get; set; }

        public string Sexo { get; set; }

        [Required]
        public DateTime dtAlteracao { get; set; }

    }
}
