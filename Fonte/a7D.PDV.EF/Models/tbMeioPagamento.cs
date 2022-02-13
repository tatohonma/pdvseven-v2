using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbMeioPagamento
    {
        public tbMeioPagamento()
        {
            this.tbTipoPagamentoes = new List<tbTipoPagamento>();
        }

        public int IDMeioPagamentoSAT { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public virtual ICollection<tbTipoPagamento> tbTipoPagamentoes { get; set; }
    }
}
