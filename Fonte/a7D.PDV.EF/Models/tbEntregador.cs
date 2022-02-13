using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbEntregador
    {
        public tbEntregador()
        {
            this.tbPedidoes = new List<tbPedido>();
        }

        public int IDEntregador { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        public bool Excluido { get; set; }
        public int? IDGateway { get; set; }
        public virtual ICollection<tbPedido> tbPedidoes { get; set; }
    }
}
