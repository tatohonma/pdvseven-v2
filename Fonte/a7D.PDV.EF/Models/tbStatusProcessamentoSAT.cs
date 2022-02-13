using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbStatusProcessamentoSAT
    {
        public tbStatusProcessamentoSAT()
        {
            this.tbProcessamentoSATs = new List<tbProcessamentoSAT>();
        }

        public int IDStatusProcessamentoSAT { get; set; }
        public string Nome { get; set; }
        public virtual ICollection<tbProcessamentoSAT> tbProcessamentoSATs { get; set; }
    }
}
