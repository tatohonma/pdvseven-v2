using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbCaixaValorRegistro
    {
        public int IDCaixaValorRegistro { get; set; }
        public int IDCaixa { get; set; }
        public int IDTipoPagamento { get; set; }
        public Nullable<decimal> ValorAbertura { get; set; }
        public Nullable<decimal> ValorFechamento { get; set; }
        public virtual tbCaixa tbCaixa { get; set; }
        public virtual tbTipoPagamento tbTipoPagamento { get; set; }
    }
}
