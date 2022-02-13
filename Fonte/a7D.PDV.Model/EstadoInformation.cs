using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbEstado")]
    [Serializable]
    public class EstadoInformation
    {
        [CRUDParameterDAL(true, "IDEstado")]
        public Int32? IDEstado { get; set; }

        [CRUDParameterDAL(false, "Nome")]
        public String Nome { get; set; }

        [CRUDParameterDAL(false, "Sigla")]
        public String Sigla { get; set; }

        public static EstadoInformation ConverterObjeto(Object obj)
        {
            return (EstadoInformation)obj;
        }
    }
}
