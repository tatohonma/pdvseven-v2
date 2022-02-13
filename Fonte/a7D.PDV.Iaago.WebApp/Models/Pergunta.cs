using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace a7D.PDV.Iaago.WebApp.Models
{
    public class Pergunta
    {
        [Key]
        [Required]
        public int ID { get; set; }

        [MaxLength(1000)]
        [Required]
        public string Texto { get; set; }

        [MaxLength(50)]
        public string Luis { get; set; }

        [MaxLength(50)]
        public string Intencao { get; set; }

        public double Score { get; set; }

        [MaxLength(50)]
        public string Entidade1 { get; set; }

        [MaxLength(100)]
        public string Valor1 { get; set; }

        [MaxLength(50)]
        public string Entidade2 { get; set; }

        [MaxLength(100)]
        public string Valor2 { get; set; }

        [MaxLength(50)]
        public string Entidade3 { get; set; }

        [MaxLength(100)]
        public string Valor3 { get; set; }

        public double QTD { get; set; }

        public DateTime Criacao { get; set; }

        public DateTime UltimaAtualizacao { get; set; }

        public DateTime UltimoUso { get; set; }

        [Column(TypeName = "TEXT")]
        public string Retorno { get; set; }

        internal object Entidades()
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(Entidade1))
            {
                result += "\n\t * " + Entidade1 + "=" + Valor1;
            }

            if (!string.IsNullOrEmpty(Entidade2))
            {
                result += "\n\t * " + Entidade2 + "=" + Valor2;
            }

            if (!string.IsNullOrEmpty(Entidade3))
            {
                result += "\n\t * " + Entidade3 + "=" + Valor3;
            }

            return result;
        }
    }
}