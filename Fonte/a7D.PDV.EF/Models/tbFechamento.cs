using a7D.PDV.Model;
using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbFechamento
    {
        public tbFechamento()
        {
            this.tbCaixas = new List<tbCaixa>();
        }

        public int IDFechamento { get; set; }
        public int IDPDV { get; set; }
        public int IDUsuario { get; set; }
        public System.DateTime DtFechamento { get; set; }
        public virtual ICollection<tbCaixa> tbCaixas { get; set; }
        public virtual PDVInformation tbPDV { get; set; }
        public virtual tbUsuario tbUsuario { get; set; }
    }
}
