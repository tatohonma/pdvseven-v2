using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace a7D.PDV.Ativacao.API.Entities
{
    [Table("tbRevenda")]
    [DataContract]
    public class Revenda
    {
        [DataMember]
        [Key]
        public int IDRevenda { get; set; }

        [Required]
        [DataMember]
        [Display(Name = "Revenda")]
        public string Nome { get; set; }

        [DataMember]
        [Required]
        public int Codigo { get; set; }
    }
}