using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace a7D.PDV.EF.Models
{
    [Table("tbIntegracao")]
    public class tbIntegracao
    {
        [Key]
        [Column(Order = 1)]
        [Required]
        public int IDTipo { get; set; }

        [Key]
        [Column(Order = 2)]
        [MaxLength(50)]
        public string Tabela { get; set; }

        [Key]
        [Column(Order = 3)]
        [Required]
        public int IDInterno { get; set; }

        [MaxLength(1)]
        public string Operacao { get; set; }

        [Required]
        [MaxLength(50)]
        public string IDExterno { get; set; }

        [Required]
        public DateTime dtMovimento { get; set; }

        public override string ToString() => $"{IDTipo}: {dtMovimento} {Tabela} {IDInterno} = {IDExterno}";
    }
}

