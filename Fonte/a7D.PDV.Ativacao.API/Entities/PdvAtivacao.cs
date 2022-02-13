using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace a7D.PDV.Ativacao.API.Entities
{
    [Table("tbPDVAtivacao")]
    [DataContract]
    public class PdvAtivacao
    {
        [Key]
        [Column(name: "IDPDVAtivacao")]
        [DataMember]
        public int IdPdvAtivacao { get; set; }

        [DataMember]
        public string ChaveAtivacao { get; set; }

        [Column(name: "IDPDV")]
        [DataMember]
        public int IdPdv { get; set; }

        [Column(name: "IDTipoPDV")]
        [DataMember]
        public int IdTipoPdv { get; set; }

        [DataMember]
        public string Nome { get; set; }

        [DataMember]
        public string ChaveHardware { get; set; }

        [DataMember]
        public DateTime? DtUltimaAlteracao { get; set; }

        [DataMember]
        public string Versao { get; set; }

        [DataMember]
        public bool Ativo { get; set; }
    }
}