using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace a7D.PDV.Gateway.UIWeb.Models
{
    public class Pedido
    {
        [Key]
        public int IdPedido { get; set; }
        public string Status { get; set; }
        public int IdTransacao { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorPago { get; set; }
        public DateTime? DataVencimentoBoleto { get; set; }
        public string UrlBoleto { get; set; }
        public string CodigoBarrasBoleto { get; set; }
        public int Parcelas { get; set; }
        public string Bandeira { get; set; }
        public string UltimosDigitosCartao { get; set; }

        public DateTime? DataAutorizacao { get; set; }
        public DateTime DataPedido { get; set; }
        public DateTime? DataPagamento { get; set; }

        public virtual List<ItemPedido> Itens { get; set; }
        public virtual List<InteracaoPagarMe> Interacoes { get; set; }
    }
}