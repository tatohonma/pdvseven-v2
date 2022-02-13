using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbMotivoCancelamento
    {
        public tbMotivoCancelamento()
        {
            this.tbPedidoProdutoes = new List<tbPedidoProduto>();
        }

        public int IDMotivoCancelamento { get; set; }
        public string Nome { get; set; }
        public virtual ICollection<tbPedidoProduto> tbPedidoProdutoes { get; set; }
    }
}
