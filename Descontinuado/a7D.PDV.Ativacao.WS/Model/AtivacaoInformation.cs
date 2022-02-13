using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.Ativacao.WS.Model
{
    [CRUDClassDAL("tbAtivacao")]
    [Serializable]
    public class AtivacaoInformation
    {
        [CRUDParameterDAL(true, "IDAtivacao")]
        public Int32? IDAtivacao { get; set; }

        [CRUDParameterDAL(false, "ChaveAtivacao")]
        public String ChaveAtivacao { get; set; }

        [CRUDParameterDAL(false, "DtUltimaVerificacao")]
        public DateTime? DtUltimaVerificacao { get; set; }

        [CRUDParameterDAL(false, "DiasValidadeAtivacao")]
        public Int32? DiasValidadeAtivacao { get; set; }

        [CRUDParameterDAL(false, "Ativo")]
        public Boolean? Ativo { get; set; }

        public static AtivacaoInformation ConverterObjeto(Object obj)
        {
            return (AtivacaoInformation)obj;
        }
    }
}