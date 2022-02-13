using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.Servico.Core
{
    public class PedidoConfig
    {
        public bool Enviado { get; set; }
        public API2.Model.Pedido Pedido { get; set; }
    }
}
