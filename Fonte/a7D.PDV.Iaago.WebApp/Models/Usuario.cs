using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace a7D.PDV.Iaago.WebApp.Models
{
    public class Usuario
    {
        [Key]
        [Required]
        public int ID { get; set; }

        [Required]
        [StringLength(20)]
        public string ChannelId { get; set; }

        [Required]
        [StringLength(200)]
        public string FromId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public DateTime UltimoAcesso { get; set; }

        [Required]
        public DateTime DataCriacao { get; set; }

        public ICollection<Variaveis> Variaveis { get; set; }
    }
}