using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbAcao")]
    [Serializable]
    public class AcaoInformation
    {
        [CRUDParameterDAL(true, "IDAcao")]
        public Int32? IDAcao { get; set; }

        [CRUDParameterDAL(false, "IDPDV", "IDPDV")]
        public PDVInformation PDV { get; set; }

        [CRUDParameterDAL(false, "Nome")]
        public String Nome { get; set; }

        [CRUDParameterDAL(false, "Parametro")]
        public String Parametro { get; set; }

        [CRUDParameterDAL(false, "DtSolicitacao")]
        public DateTime? DtSolicitacao { get; set; }

        public static AcaoInformation ConverterObjeto(Object obj)
        {
            return (AcaoInformation)obj;
        }
    }
}
