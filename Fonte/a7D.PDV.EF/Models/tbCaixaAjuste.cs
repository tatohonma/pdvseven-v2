using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbCaixaAjuste
    {
        public int IDCaixaAjuste { get; set; }
        public int IDCaixa { get; set; }
        public int IDCaixaTipoAjuste { get; set; }
        public decimal Valor { get; set; }
        public string Descricao { get; set; }
        public System.DateTime DtAjuste { get; set; }
        public virtual tbCaixa tbCaixa { get; set; }
        public virtual tbCaixaTipoAjuste tbCaixaTipoAjuste { get; set; }
    }
}
