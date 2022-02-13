using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Model.Dashboard.DTO
{

    [DataContract]
    public class Fechamento
    {
        [DataMember(Name = "semFechamento", EmitDefaultValue = true)]
        public bool SemFechamento { get; set; } = true;

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "abertura")]
        public DateTime Abertura { get; set; }

        [DataMember(Name = "fechamentos")]
        public List<FechamentoFiltro> Fechamentos { get; set; }

        [DataMember(Name = "fechamento")]
        public DateTime _Fechamento { get; set; }

        [DataMember(Name = "faturamentoTotal")]
        public decimal FaturamentoTotal { get; set; }

        [DataMember(Name = "totalPedidos")]
        public int TotalPedidos { get; set; }

        [DataMember(Name = "descontos")]
        public decimal Descontos { get; set; }

        [DataMember(Name = "cancelamentos")]
        public decimal Cancelamentos { get; set; }

        [DataMember(Name = "tipoPagamento")]
        public List<TipoPagamento> TipoPagamento { get; set; }

        [DataMember(Name = "garcom")]
        public List<Garcom> Garcom { get; set; }

        [DataMember(Name = "tipoPedido")]
        public List<TipoPedido> TipoPedido { get; set; }

        [DataMember(Name = "caixa")]
        public List<Caixa> Caixa { get; set; }
    }

    [DataContract]
    public partial class FechamentoFiltro
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "nome")]
        public string Nome { get; set; }
    }

    [DataContract]
    public partial class Caixa
    {
        [DataMember(Name = "nome")]
        public string Nome { get; set; }

        [DataMember(Name = "usuario")]
        public string Usuario { get; set; }

        [DataMember(Name = "valor")]
        public decimal Valor { get; set; }

        [DataMember(Name = "diferenca")]
        public decimal Diferenca { get; set; }
    }

    [DataContract]
    public partial class TipoPedido
    {
        [DataMember(Name = "tipo")]
        public string Tipo { get; set; }

        [DataMember(Name = "valor")]
        public decimal Valor { get; set; }

        [DataMember(Name = "quantidade")]
        public int Quantidade { get; set; }
    }

    [DataContract]
    public partial class Garcom
    {
        [DataMember(Name = "nome")]
        public string Nome { get; set; }

        [DataMember(Name = "valor")]
        public decimal Valor { get; set; }

        [DataMember(Name = "percentual")]
        public decimal Percentual { get; set; }
    }

    [DataContract]
    public partial class TipoPagamento
    {
        [DataMember(Name = "tipo")]
        public string Tipo { get; set; }

        [DataMember(Name = "valor")]
        public decimal Valor { get; set; }

        [DataMember(Name = "percentual")]
        public decimal Percentual { get; set; }
    }
}
