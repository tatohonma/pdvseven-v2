using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbEstado
    {
        public tbEstado()
        {
            this.tbClientes = new List<tbCliente>();
        }

        public int IDEstado { get; set; }
        public string Nome { get; set; }
        public string Sigla { get; set; }
        public virtual ICollection<tbCliente> tbClientes { get; set; }
    }
}
