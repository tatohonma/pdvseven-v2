using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace a7D.PDV.Ativacao.API.Entities
{
    [Table("tbPDV")]
    [DataContract]
    public class PDV
    {
        [Key]
        [DataMember]
        public int IDPDV { get; set; }

        [Required]
        [DataMember]
        public int IDAtivacao { get; set; }

        public virtual Ativacao Ativacao { get; set; }

        [DataMember]
        public int? IDPDV_instalacao { get; set; }

        [Required]
        [Display(Name = "Tipo PDV")]
        [DataMember]
        public int IDTipoPDV { get; set; }

        public virtual TipoPDV TipoPDV { get; set; }

        [DataMember]
        public string Nome { get; set; }

        [Display(Name = "Chave Hardware")]
        [DataMember]
        public string ChaveHardware { get; set; }

        [Display(Name = "Data Última Alteração")]
        [DataMember]
        public DateTime? DtUltimaAlteracao { get; set; }

        [DataMember]
        public bool Ativo { get; set; }

        [DataMember]
        public string Versao { get; set; }

        public string Insert(int proximoId)
        {
            var insert = string.Format("INSERT [dbo].[tbPDV] ([IDPDV], [IDTipoPDV], [ChaveHardware], [Nome], [UltimoAcesso], [Ativo]) VALUES ({0}, {1}, NULL, '{2}', NULL, 1)"
                , proximoId, IDTipoPDV, Nome);

            return insert;
        }
    }
}