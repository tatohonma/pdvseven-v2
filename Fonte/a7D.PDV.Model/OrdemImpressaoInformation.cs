using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbOrdemImpressao")]
    [Serializable]
    public class OrdemImpressaoInformation
    {
        [CRUDParameterDAL(true, "IDOrdemImpressao")]
        public Int32? IDOrdemImpressao { get; set; }

        [CRUDParameterDAL(false, "IDAreaImpressao", "IDAreaImpressao")]
        public AreaImpressaoInformation AreaImpressao { get; set; }

        [CRUDParameterDAL(false, "ConteudoImpressao")]
        public String ConteudoImpressao { get; set; }

        [CRUDParameterDAL(false, "DtOrdem")]
        public DateTime? DtOrdem { get; set; }

        [CRUDParameterDAL(false, "IDTipoOrdemImpressao")]
        public int IDTipoOrdemImpressao { get; set; }

        //[CRUDParameterDAL(false, "EnviadoFilaImpressao")]
        //public Boolean? EnviadoFilaImpressao { get; set; } // TODO: Remover

        //[CRUDParameterDAL(false, "Conta")]
        //public bool? Conta { get; set; }

        //[CRUDParameterDAL(false, "SAT")]
        //public bool? SAT { get; set; }

        public static OrdemImpressaoInformation ConverterObjeto(Object obj)
        {
            return (OrdemImpressaoInformation)obj;
        }
    }
}