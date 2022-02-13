using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbPDV")]
    [Serializable]
    class PDV2Information
    {
        [CRUDParameterDAL(true, "IDPDV")]
        public Int32? IDPDV { get; set; }

        [CRUDParameterDAL(false, "IDTipoPDV", "IDTipoPDV")]
        public TipoPDVInformation TipoPDV { get; set; }

        [CRUDParameterDAL(false, "ChaveHardware")]
        public String ChaveHardware { get; set; }

        [CRUDParameterDAL(false, "Nome")]
        public String Nome { get; set; }

        [CRUDParameterDAL(false, "UltimoAcesso")]
        public DateTime? UltimoAcesso { get; set; }

        [CRUDParameterDAL(false, "UltimaAlteracao")]
        public DateTime? UltimaAlteracao { get; set; }

        [CRUDParameterDAL(false, "Versao")]
        public string Versao { get; set; }

        public static PDVInformation ConverterObjeto(Object obj)
        {
            return (PDVInformation)obj;
        }
    }
}
