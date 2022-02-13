using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbStatusPedido : IValueName
    {
        public tbStatusPedido()
        {
            this.tbPedidoes = new List<tbPedido>();
        }

        public int IDStatusPedido { get; set; }
        public string Nome { get; set; }
        public virtual ICollection<tbPedido> tbPedidoes { get; set; }

        public ValueName GetValueName() => new ValueName(IDStatusPedido, Nome);

        public void SetValueName(int value, string name)
        {
            IDStatusPedido = value;
            Nome = name;
        }
    }
}
