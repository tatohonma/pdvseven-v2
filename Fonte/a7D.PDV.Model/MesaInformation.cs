using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbMesa")]
    [Serializable]
    public class MesaInformation
    {
        [CRUDParameterDAL(true, "IDMesa")]
        public Int32? IDMesa { get; set; }

        [CRUDParameterDAL(false, "IDStatusMesa", "IDStatusMesa")]
        public StatusMesaInformation StatusMesa { get; set; }

        [CRUDParameterDAL(false, "GUIDIdentificacao")]
        public String GUIDIdentificacao { get; set; }

        [CRUDParameterDAL(false, "Numero")]
        public Int32? Numero { get; set; }

        [CRUDParameterDAL(false, "Capacidade")]
        public Int32? Capacidade { get; set; }

        public static MesaInformation ConverterObjeto(Object obj)
        {
            return (MesaInformation)obj;
        }
    }
}
