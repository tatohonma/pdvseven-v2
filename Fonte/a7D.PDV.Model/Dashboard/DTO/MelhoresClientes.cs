using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Model.Dashboard.DTO
{
    [DataContract]
    public class MelhoresClientes
    {
        [DataMember(Name = "totalDeVendas")]
        public List<Cliente> TotalDeVendas { get; set; }
    }

    [DataContract]
    public partial class Cliente
    {
        [DataMember(Name = "ranking")]
        public int Ranking { get; set; }

        [DataMember(Name = "nome")]
        public string Nome { get; set; }

        [DataMember(Name = "totalVendas")]
        public decimal TotalVendas { get; set; }

        [DataMember(Name = "qtdVisitas")]
        public long QtdVisitas { get; set; }

        [DataMember(Name = "ticketMedio")]
        public decimal TicketMedio { get; set; }

        [DataMember(Name = "ultimaVisita")]
        public DateTime UltimaVisita { get; set; }
    }
}
