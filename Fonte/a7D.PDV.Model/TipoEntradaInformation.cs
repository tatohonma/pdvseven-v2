using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbTipoEntrada")]
    [Serializable]
    public class TipoEntradaInformation
    {
        [CRUDParameterDAL(true, "IDTipoEntrada")]
        public Int32? IDTipoEntrada { get; set; }

        [CRUDParameterDAL(false, "Nome")]
        public String Nome { get; set; }

        [CRUDParameterDAL(false, "ValorEntrada")]
        public Decimal? ValorEntrada { get; set; }

        [CRUDParameterDAL(false, "ValorConsumacaoMinima")]
        public Decimal? ValorConsumacaoMinima { get; set; }

        [CRUDParameterDAL(false, "Padrao")]
        public Boolean? Padrao { get; set; }

        [CRUDParameterDAL(false, "Ativo")]
        public Boolean? Ativo { get; set; }

        public static TipoEntradaInformation ConverterObjeto(Object obj)
        {
            return (TipoEntradaInformation)obj;
        }
    }
}
