using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbRelatorio")]
    [Serializable]
    public class RelatorioInformation
    {
        [CRUDParameterDAL(true, "IDRelatorio")]
        public Int32? IDRelatorio { get; set; }

        [CRUDParameterDAL(false, "IDTipoRelatorio", "IDTipoRelatorio")]
        public TipoRelatorioInformation TipoRelatorio { get; set; }

        [CRUDParameterDAL(false, "Nome")]
        public String Nome { get; set; }

        [CRUDParameterDAL(false, "Descricao")]
        public String Descricao { get; set; }

        [CRUDParameterDAL(false, "QuerySQL")]
        public String QuerySQL { get; set; }

        [CRUDParameterDAL(false, "Ordem")]
        public Int32? Ordem { get; set; }

        [CRUDParameterDAL(false, "Totalizador")]
        public string Totalizador { get; set; }

        public static RelatorioInformation ConverterObjeto(Object obj)
        {
            return (RelatorioInformation)obj;
        }
    }
}
