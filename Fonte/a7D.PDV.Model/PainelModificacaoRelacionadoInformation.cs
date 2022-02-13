using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbPainelModificacaoRelacionado")]
    [Serializable]
    public class PainelModificacaoRelacionadoInformation
    {
        [CRUDParameterDAL(true, "IDPainelModificacaoRelacionado")]
        public Int32? IDPainelModificacaoRelacionado { get; set; }

        [CRUDParameterDAL(false, "IDPainelModificacao1", "IDPainelModificacao")]
        public PainelModificacaoInformation PainelModificacao1 { get; set; }

        [CRUDParameterDAL(false, "IDPainelModificacao2", "IDPainelModificacao")]
        public PainelModificacaoInformation PainelModificacao2 { get; set; }

        //[CRUDParameterDAL(false, "Ordem")]
        //public int? Ordem { get; set; }

        public StatusModel StatusModel { get; set; }

        public static PainelModificacaoInformation ConverterObjeto(Object obj)
        {
            return (PainelModificacaoInformation)obj;
        }
    }
}
