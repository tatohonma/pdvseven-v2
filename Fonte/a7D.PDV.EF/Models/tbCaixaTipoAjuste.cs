using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbCaixaTipoAjuste
    {
        public tbCaixaTipoAjuste()
        {
            this.tbCaixaAjustes = new List<tbCaixaAjuste>();
        }

        public int IDCaixaTipoAjuste { get; set; }
        public string Nome { get; set; }
        public virtual ICollection<tbCaixaAjuste> tbCaixaAjustes { get; set; }
    }
}
