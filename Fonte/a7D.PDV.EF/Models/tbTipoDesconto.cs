using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbTipoDesconto
    {
        public tbTipoDesconto()
        {
            this.tbPedidoes = new List<tbPedido>();
            this.tbPedidoProdutoes = new List<tbPedidoProduto>();
        }

        public int IDTipoDesconto { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal? LimiteValor { get; set; }
        public decimal? LimitePercentual { get; set; }
        public int? NivelAcesso { get; set; }
        public bool Ativo { get; set; }
        public bool Excluido { get; set; }

        public virtual ICollection<tbPedido> tbPedidoes { get; set; }
        public virtual ICollection<tbPedidoProduto> tbPedidoProdutoes { get; set; }
    }
}
