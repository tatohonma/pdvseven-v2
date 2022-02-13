using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbMesa
    {
        public int IDMesa { get; set; }
        public int IDStatusMesa { get; set; }
        public string GUIDIdentificacao { get; set; }
        public int Numero { get; set; }
        public Nullable<int> Capacidade { get; set; }
        public virtual tbStatusMesa tbStatusMesa { get; set; }
    }
}
