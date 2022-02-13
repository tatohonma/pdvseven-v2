using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace a7D.PDV.Gateway.UIWeb.Models
{
    public class ItemPedido
    {
        [Key]
        public int IdItemPedido { get; set; }
        public int IdPedido { get; set; }
        public int IdContaReceber { get; set; }

        public virtual Pedido Pedido { get; set; }
        public virtual ContaReceber ContaReceber { get; set; }
    }
}