using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbTipoRelatorio")]
    [Serializable]
    public class TipoRelatorioInformation
    {
        [CRUDParameterDAL(true, "IDTipoRelatorio")]
        public Int32? IDTipoRelatorio { get; set; }

        [CRUDParameterDAL(false, "Nome")]
        public String Nome { get; set; }

        public static TipoRelatorioInformation ConverterObjeto(Object obj)
        {
            return (TipoRelatorioInformation)obj;
        }
    }
}
