using a7D.Fmk.CRUD.DAL;
using System;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbStatusMesa")]
    [Serializable]
    public class StatusMesaInformation
    {
        [CRUDParameterDAL(true, "IDStatusMesa")]
        public Int32? IDStatusMesa { get; set; }

        [CRUDParameterDAL(false, "Nome")]
        public String Nome { get; set; }

        public static StatusMesaInformation ConverterObjeto(Object obj)
        {
            return (StatusMesaInformation)obj;
        }
    }
}
