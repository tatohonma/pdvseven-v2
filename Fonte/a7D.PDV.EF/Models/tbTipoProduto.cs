using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbTipoProduto : IValueName
    {
        public tbTipoProduto()
        {
            this.tbProdutoes = new List<tbProduto>();
        }

        public int IDTipoProduto { get; set; }
        public string Nome { get; set; }
        public virtual ICollection<tbProduto> tbProdutoes { get; set; }

        public ValueName GetValueName() => new ValueName(IDTipoProduto, Nome);

        public void SetValueName(int value, string name)
        {
            IDTipoProduto = value;
            Nome = name;
        }
    }
}
