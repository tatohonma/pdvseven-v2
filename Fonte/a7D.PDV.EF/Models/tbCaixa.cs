using a7D.PDV.Model;
using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbCaixa
    {
        public tbCaixa()
        {
            this.tbCaixaAjustes = new List<tbCaixaAjuste>();
            this.tbCaixaValorRegistroes = new List<tbCaixaValorRegistro>();
            this.tbPedidoes = new List<tbPedido>();
        }

        public int IDCaixa { get; set; }
        public int IDPDV { get; set; }
        public Nullable<int> IDUsuario { get; set; }
        public Nullable<int> IDFechamento { get; set; }
        public System.DateTime DtAbertura { get; set; }
        public Nullable<System.DateTime> DtFechamento { get; set; }
        public bool SincERP { get; set; }
        public virtual tbFechamento tbFechamento { get; set; }
        public virtual PDVInformation tbPDV { get; set; }
        public virtual tbUsuario tbUsuario { get; set; }
        public virtual ICollection<tbCaixaAjuste> tbCaixaAjustes { get; set; }
        public virtual ICollection<tbCaixaValorRegistro> tbCaixaValorRegistroes { get; set; }
        public virtual ICollection<tbPedido> tbPedidoes { get; set; }
    }
}
