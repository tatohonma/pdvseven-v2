using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace a7D.PDV.Gateway.UIWeb.Models
{
    public class ContaReceber
    {
        [Key]
        public int IdContaReceber { get; set; }
        public DateTime Data { get; set; }
        public DateTime Vencimento { get; set; }
        public decimal Valor { get; set; }
        public decimal Saldo { get; set; }
        public string IdErp { get; set; }
        public string Historico { get; set; }
        public string IdBroker { get; set; }
        public int IdCliente { get; set; }
        public string Categoria { get; set; }
        public bool Pendente { get; set; }

        public virtual Cliente Cliente { get; set; }

        public override string ToString()
        {
            return $@"
{{
    IdContaReceber: ""{IdContaReceber}"",
    Data: ""{Data.ToString()}"",
    Vencimento: ""{Vencimento.ToString()}"",
    Valor: ""{Valor}"",
    Saldo: ""{Saldo}"",
    IdErp: ""{IdErp}"",
    Historico: ""{Historico}"",
    IdBroker: ""{IdBroker}"",
    IdCliente: ""{IdCliente}"",
    Categoria: ""{Categoria}"",
    Pendente: ""{Pendente}""
}}
            ";
        }
    }
}