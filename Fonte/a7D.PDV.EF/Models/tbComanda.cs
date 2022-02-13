using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbComanda
    {
        public int IDComanda { get; set; }
        public int IDStatusComanda { get; set; }
        public string GUIDIdentificacao { get; set; }
        public int Numero { get; set; }
        public Int64? Codigo { get; set; }
        public Int32? IDCliente { get; set; }
        public string Observacao { get; set; }
        public virtual tbStatusComanda tbStatusComanda { get; set; }
    }
}
