using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Model.Dashboard.DTO
{
    [DataContract]
    public class Resumo
    {
        [DataMember(Name = "totalVendas")]
        public decimal TotalVendas { get; set; }

        [DataMember(Name = "pedido")]
        public TicketMedio Pedido { get; set; }

        [DataMember(Name = "cliente")]
        public TicketMedio Cliente { get; set; }
    }

    [DataContract]
    public partial class TicketMedio
    {
        [DataMember(Name = "total")]
        public decimal Total { get; set; }

        [DataMember(Name = "quantidade")]
        public decimal Quantidade { get; set; }

        [DataMember(Name = "mesa")]
        public Ticket Mesa { get; set; }

        [DataMember(Name = "comanda")]
        public Ticket Comanda { get; set; }

        [DataMember(Name = "delivery")]
        public Ticket Delivery { get; set; }

        [DataMember(Name = "balcao")]
        public Ticket Balcao { get; set; }
    }

    [DataContract]
    public partial class Ticket
    {
        [DataMember(Name = "valor")]
        public decimal Valor { get; set; }

        [DataMember(Name = "quantidade")]
        public decimal Quantidade { get; set; }
    }
}
