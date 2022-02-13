using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbConfiguracao")]
    [Serializable]
    public class ConfiguracaoInformation
    {
        [CRUDParameterDAL(true, "Chave")]
        public String Chave { get; set; }

        [CRUDParameterDAL(false, "Valor")]
        public String Valor { get; set; }

        public static ConfiguracaoInformation ConverterObjeto(Object obj)
        {
            return (ConfiguracaoInformation)obj;
        }
    }
}
