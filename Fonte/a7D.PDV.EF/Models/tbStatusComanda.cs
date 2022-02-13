using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbStatusComanda : IValueName
    {
        public tbStatusComanda()
        {
            this.tbComandas = new List<tbComanda>();
        }

        public int IDStatusComanda { get; set; }
        public string Nome { get; set; }
        public virtual ICollection<tbComanda> tbComandas { get; set; }

        public ValueName GetValueName() => new ValueName(IDStatusComanda, Nome);

        public void SetValueName(int value, string name)
        {
            IDStatusComanda = value;
            Nome = name;
        }
    }
}
