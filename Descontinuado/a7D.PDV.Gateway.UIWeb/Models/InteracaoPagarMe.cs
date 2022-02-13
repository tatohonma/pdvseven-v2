using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace a7D.PDV.Gateway.UIWeb.Models
{
    public class InteracaoPagarMe
    {
        [Key]
        public int IdInteracaoPagarMe { get; set; }
        public string Conteudo { get; set; }
        public int? IdPedido { get; set; }
        public bool Capturado { get; set; }
        public string Erro { get; set; }

        public virtual Pedido Pedido { get; set; }
    }
}