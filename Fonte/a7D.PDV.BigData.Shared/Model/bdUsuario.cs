using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace a7D.PDV.BigData.Shared.Model
{
    public class bdUsuario
    {
        [Key, Column(Order = 2), Required]
        public int IDUsuario { get; set; }

        public string Nome { get; set; }

        public string Senha { get; set; }

        public bool Ativo { get; set; }

        [Required]
        public DateTime dtAlteracao { get; set; }
    }
}
