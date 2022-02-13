using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbCaixaTipoAjuste")]
    [Serializable]
    public class CaixaTipoAjusteInformation
    {
        [CRUDParameterDAL(true, "IDCaixaTipoAjuste")]
        public Int32? IDCaixaTipoAjuste { get; set; }

        [CRUDParameterDAL(false, "Nome")]
        public String Nome { get; set; }

        public static CaixaTipoAjusteInformation ConverterObjeto(Object obj)
        {
            return (CaixaTipoAjusteInformation)obj;
        }
    }
}
