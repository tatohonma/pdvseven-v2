using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbTipoRelatorio
    {
        public tbTipoRelatorio()
        {
            this.tbRelatorios = new List<tbRelatorio>();
        }

        public int IDTipoRelatorio { get; set; }
        public string Nome { get; set; }
        public virtual ICollection<tbRelatorio> tbRelatorios { get; set; }
    }
}
