using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace a7D.PDV.Gateway.UIWeb.Models
{
    public class Cliente
    {
        [Key]
        public int IdCliente { get; set; }
        public string IdBroker { get; set; }
        public string Nome { get; set; }
        public string CpfCnpj { get; set; }
    }
}