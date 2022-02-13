using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace a7D.PDV.Ativacao.API.Entities
{
    [Table("tbTipoPDV")]
    [DataContract]
    public class TipoPDV
    {
        [Key]
        [DataMember]
        public int IDTipoPDV { get; set; }

        [DataMember]
        public string Nome { get; set; }
    }
}