using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbCodigoSAT")]
    [Serializable]
    public class CodigoSATInformation
    {
        [CRUDParameterDAL(true, "IDCodigoSAT")]
        public Int32? IDCodigoSAT { get; set; }

        [CRUDParameterDAL(false, "CodigoRetorno")]
        public String CodigoRetorno { get; set; }

        [CRUDParameterDAL(false, "Grupo")]
        public String Grupo { get; set; }

        [CRUDParameterDAL(false, "Mensagem")]
        public String Mensagem { get; set; }

        [CRUDParameterDAL(false, "Descricao")]
        public String Descricao { get; set; }

        [CRUDParameterDAL(false, "Erro")]
        public Boolean? Erro { get; set; }

        public static CodigoSATInformation ConverterObjeto(Object obj)
        {
            return (CodigoSATInformation)obj;
        }
    }
}
