using a7D.PDV.EF.Enum;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace a7D.PDV.EF.Models
{
    public partial class tbOrdemImpressao
    {
        public int IDOrdemImpressao { get; set; }
        public int IDAreaImpressao { get; set; }
        public string ConteudoImpressao { get; set; }
        public DateTime DtOrdem { get; set; }
        public int IDTipoOrdemImpressao { get; set; }

        [NotMapped]
        public ETipoOrdemImpressao TipoOrdemImpressao
        {
            get { return (ETipoOrdemImpressao)IDTipoOrdemImpressao; }
            set { IDTipoOrdemImpressao = (int)value; }
        }

        //public bool EnviadoFilaImpressao { get; set; }
        //public bool Conta { get; set; }
        //public bool SAT { get; set; }
        public virtual tbAreaImpressao tbAreaImpressao { get; set; }
    }
}
