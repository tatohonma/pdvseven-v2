using a7D.Fmk.CRUD.DAL;
using a7D.PDV.EF.Enum;
using System;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbTag")]
    [Serializable]
    public class TagInformation
    {
        [CRUDParameterDAL(true, "IDTag")]
        public Int32? IDTag { get; set; }

        [CRUDParameterDAL(false, "GUIDIdentificacao")]
        public String GUIDIdentificacao { get; set; }

        [CRUDParameterDAL(false, "Chave")]
        public String Chave { get; set; }

        [CRUDParameterDAL(false, "Valor")]
        public String Valor { get; set; }

        [CRUDParameterDAL(false, "DtInclusao")]
        public DateTime? DtInclusao { get; set; }

        public static TagInformation ConverterObjeto(Object obj)
        {
            return (TagInformation)obj;
        }
    }
}
