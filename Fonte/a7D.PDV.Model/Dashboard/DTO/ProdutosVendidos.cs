using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Model.Dashboard.DTO
{
    [DataContract]
    public partial class ProdutosVendidos
    {
        [DataMember(Name = "valorTotal")]
        public decimal ValorTotal { get; set; }

        [DataMember(Name = "maisVendidos")]
        public List<Produto> MaisVendidos { get; set; }

        [DataMember(Name = "menosVendidos")]
        public List<Produto> MenosVendidos { get; set; }
    }

    [DataContract]
    public partial class Produto
    {
        [DataMember(Name = "produto")]
        public string _Produto { get; set; }

        [DataMember(Name = "qtd")]
        public decimal Qtd { get; set; }

        [DataMember(Name = "valor")]
        public decimal Valor { get; set; }

        [DataMember(Name = "percentual")]
        public decimal Percentual { get; set; }

        [DataMember(Name = "ranking")]
        public int Ranking { get; set; }
    }
}
