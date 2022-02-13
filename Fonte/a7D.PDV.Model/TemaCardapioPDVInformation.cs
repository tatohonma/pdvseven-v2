using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbTemaCardapioPDV")]
    [Serializable]
    public class TemaCardapioPDVInformation
    {
        [CRUDParameterDAL(true, "IDTemaCardapioPDV")]
        public Int32? IDTemaCardapioPDV { get; set; }

        [CRUDParameterDAL(false, "IDTemaCardapio", "IDTemaCardapio")]
        public TemaCardapioInformation TemaCardapio { get; set; }

        [CRUDParameterDAL(false, "IDPDV", "IDPDV")]
        public PDVInformation PDV { get; set; }

        [CRUDParameterDAL(false, "Ativo")]
        public Boolean? Ativo { get; set; }

        [CRUDParameterDAL(false, "DtUltimaAlteracao")]
        public DateTime? DtUltimaAlteracao { get; set; }

        public static TemaCardapioPDVInformation ConverterObjeto(Object obj)
        {
            return (TemaCardapioPDVInformation)obj;
        }
    }
}
