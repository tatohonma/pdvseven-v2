using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbCaixaValorRegistro")]
    [Serializable]
    public class CaixaValorRegistroInformation
    {
        [CRUDParameterDAL(true, "IDCaixaValorRegistro")]
        public Int32? IDCaixaValorRegistro { get; set; }

        [CRUDParameterDAL(false, "IDCaixa", "IDCaixa")]
        public CaixaInformation Caixa { get; set; }

        [CRUDParameterDAL(false, "IDTipoPagamento", "IDTipoPagamento")]
        public TipoPagamentoInformation TipoPagamento { get; set; }

        [CRUDParameterDAL(false, "ValorAbertura")]
        public Decimal? ValorAbertura { get; set; }

        [CRUDParameterDAL(false, "ValorFechamento")]
        public Decimal? ValorFechamento { get; set; }

        public static CaixaValorRegistroInformation ConverterObjeto(Object obj)
        {
            return (CaixaValorRegistroInformation)obj;
        }
    }
}
