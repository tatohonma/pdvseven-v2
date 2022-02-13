using a7D.PDV.Ativacao.API.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace a7D.PDV.Ativacao.API.Entities
{
    [Table("tbAtivacao")]
    [DataContract]
    public class Ativacao
    {
        [Key]
        [DataMember]
        public int IDAtivacao { get; set; }

        [Required]
        [Display(Name = "Cliente")]
        [DataMember]

        public int IDCliente { get; set; }

        [Display(Name = "Cliente")]
        [DataMember]
        public virtual Cliente Cliente { get; set; }

        [Display(Name = "Chave Ativação")]
        [DataMember]
        [Required]
        public string ChaveAtivacao { get; set; }

        [DataMember]
        [Display(Name = "Ultima Verificação")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DtUltimaVerificacao { get; set; }

        [DataMember]
        public DateTime? DataAtivacao { get; set; }

        [Display(Name = "Dias de Validade")]
        [DataMember]
        [Required]
        public int DiasValidadeAtivacao { get; set; }

        [DataMember]
        public bool Ativo { get; set; }

        [DataType(DataType.MultilineText)]
        [DataMember]
        public string Observacao { get; set; }

        [DataMember]
        public bool ReativadoSuporte { get; set; }

        [DataMember]
        public DateTime? DataReativacaoSuporte { get; set; }

        [DataMember]
        public DateTime? DataValidadeProvisoria { get; set; }

        [DataMember]
        public bool Duplicidade { get; set; }

        [DataMember]
        [NotMapped]
        public bool? SiteAdmin { get; set; }

       [DataMember]
        [NotMapped]
        public string Licencas
        {
            get
            {
                if (PDVs == null || PDVs.Count == 0)
                    return "";

                if (TipoPDVRepository.Lista == null)
                    TipoPDVRepository.PreencheLista();

                bool notFound = false;
                var tipoPDV = TipoPDVRepository.Lista;
                var result = "";
                var gb = PDVs.GroupBy(p => p.IDTipoPDV, (key, value) => new { IDTipoPDV = key, Quantidade = value.Count() });

                foreach (var g in gb)
                {
                    var tipo = tipoPDV?.FirstOrDefault(t => t.IDTipoPDV == g.IDTipoPDV);
                    if (tipo == null)
                        notFound = true;
                    else
                        result += string.Format("{0} {1}<br />", g.Quantidade, tipo.Nome);
                }

                if (notFound)
                    TipoPDVRepository.Lista = null;

                return result;
            }
            set { }
        }

        [DataMember]
        public virtual List<PDV> PDVs { get; set; }
    }
}