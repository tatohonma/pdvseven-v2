using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace a7D.PDV.Iaago.WebApp.Models
{
    public class Variaveis
    {
        [Key]
        [Required]
        public int ID { get; set; }

        [ForeignKey("Usuario")]
        public int IDUsuario { get; set; }

        public virtual Usuario Usuario { get; set; }

        [MaxLength(100)]
        [Required]
        public string Chave { get; set; }

        [MaxLength(1000)]
        [Required]
        public string Valor { get; set; }

        [Required]
        public DateTime DataGravacao { get; set; }
    }
}