using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbCaixaAjuste")]
    [Serializable]
    public class CaixaAjusteInformation
    {
        [CRUDParameterDAL(true, "IDCaixaAjuste")]
        public Int32? IDCaixaAjuste { get; set; }

        [CRUDParameterDAL(false, "IDCaixa", "IDCaixa")]
        public CaixaInformation Caixa { get; set; }

        [CRUDParameterDAL(false, "IDCaixaTipoAjuste", "IDCaixaTipoAjuste")]
        public CaixaTipoAjusteInformation CaixaTipoAjuste { get; set; }

        [CRUDParameterDAL(false, "Valor")]
        public Decimal? Valor { get; set; }

        [CRUDParameterDAL(false, "Descricao")]
        public String Descricao { get; set; }

        [CRUDParameterDAL(false, "DtAjuste")]
        public DateTime? DtAjuste { get; set; }

        public static CaixaAjusteInformation ConverterObjeto(Object obj)
        {
            return (CaixaAjusteInformation)obj;
        }
    }
}
