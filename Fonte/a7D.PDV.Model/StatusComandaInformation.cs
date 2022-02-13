using a7D.Fmk.CRUD.DAL;
using System;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbStatusComanda")]
    [Serializable]
    public class StatusComandaInformation
    {
        [CRUDParameterDAL(true, "IDStatusComanda")]
        public Int32 IDStatusComanda { get; set; }

        [CRUDParameterDAL(false, "Nome")]
        public String Nome { get; set; }

        public static StatusComandaInformation ConverterObjeto(Object obj)
        {
            return (StatusComandaInformation)obj;
        }
    }
}
