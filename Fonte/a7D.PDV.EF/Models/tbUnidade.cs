using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbUnidade
    {
        public tbUnidade()
        {
            this.tbConversaoUnidades = new List<tbConversaoUnidade>();
            this.tbConversaoUnidades1 = new List<tbConversaoUnidade>();
            this.tbInventarioProdutos = new List<tbInventarioProduto>();
            this.tbMovimentacaoProdutos = new List<tbMovimentacaoProduto>();
            this.tbProdutoes = new List<tbProduto>();
            this.tbProdutoReceitas = new List<tbProdutoReceita>();
        }

        public int IDUnidade { get; set; }
        public string Nome { get; set; }
        public string Simbolo { get; set; }
        public bool Excluido { get; set; }
        public virtual ICollection<tbConversaoUnidade> tbConversaoUnidades { get; set; }
        public virtual ICollection<tbConversaoUnidade> tbConversaoUnidades1 { get; set; }
        public virtual ICollection<tbInventarioProduto> tbInventarioProdutos { get; set; }
        public virtual ICollection<tbMovimentacaoProduto> tbMovimentacaoProdutos { get; set; }
        public virtual ICollection<tbProduto> tbProdutoes { get; set; }
        public virtual ICollection<tbProdutoReceita> tbProdutoReceitas { get; set; }
    }
}
