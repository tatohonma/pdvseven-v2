using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbStatusMesa : IValueName
    {
        public tbStatusMesa()
        {
            this.tbMesas = new List<tbMesa>();
        }

        public int IDStatusMesa { get; set; }
        public string Nome { get; set; }
        public virtual ICollection<tbMesa> tbMesas { get; set; }

        public ValueName GetValueName() => new ValueName(IDStatusMesa, Nome);

        public void SetValueName(int value, string name)
        {
            IDStatusMesa = value;
            Nome = name;
        }
    }
}
