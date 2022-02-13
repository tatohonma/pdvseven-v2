using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbMotivoCancelamento")]
    [Serializable]
    public class MotivoCancelamentoInformation
    {
        [CRUDParameterDAL(true, "IDMotivoCancelamento")]
        public Int32? IDMotivoCancelamento { get; set; }

        [CRUDParameterDAL(false, "Nome")]
        public String Nome { get; set; }

        public static MotivoCancelamentoInformation ConverterObjeto(Object obj)
        {
            return (MotivoCancelamentoInformation)obj;
        }
    }
}
