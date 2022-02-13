using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbFechamento")]
    [Serializable]
    public class FechamentoInformation
    {
        [CRUDParameterDAL(true, "IDFechamento")]
        public Int32? IDFechamento { get; set; }

        [CRUDParameterDAL(false, "IDPDV", "IDPDV")]
        public PDVInformation PDV { get; set; }

        [CRUDParameterDAL(false, "IDUsuario", "IDUsuario")]
        public UsuarioInformation Usuario { get; set; }
        
        [CRUDParameterDAL(false, "DtFechamento")]
        public DateTime? DtFechamento { get; set; }
        
        public static FechamentoInformation ConverterObjeto(Object obj)
        {
            return (FechamentoInformation)obj;
        }
    }
}
