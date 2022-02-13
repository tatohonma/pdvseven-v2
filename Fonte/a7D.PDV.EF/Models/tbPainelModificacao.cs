using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbPainelModificacao
    {
        public tbPainelModificacao()
        {
            this.tbPainelModificacaoCategorias = new List<tbPainelModificacaoCategoria>();
            this.tbPainelModificacaoRelacionadoes = new List<tbPainelModificacaoRelacionado>();
            this.tbPainelModificacaoRelacionadoes1 = new List<tbPainelModificacaoRelacionado>();
            this.tbPedidoProdutoes = new List<tbPedidoProduto>();
            this.tbPainelModificacaoProdutoes = new List<tbPainelModificacaoProduto>();
            this.tbProdutoPainelModificacaos = new List<tbProdutoPainelModificacao>();
        }

        public int IDPainelModificacao { get; set; }
        public string Nome { get; set; }
        public string Titulo { get; set; }
        public Nullable<int> Minimo { get; set; }
        public Nullable<int> Maximo { get; set; }
        public System.DateTime DtUltimaAlteracao { get; set; }
        public bool Excluido { get; set; }
        public Nullable<int> IDPainelModificacaoOperacao { get; set; }
        public Nullable<int> IDTipoItem { get; set; }
        public Nullable<int> IDValorUtilizado { get; set; }
        public Nullable<bool> IgnorarValorItem { get; set; }
        public virtual ICollection<tbPainelModificacaoCategoria> tbPainelModificacaoCategorias { get; set; }
        public virtual ICollection<tbPainelModificacaoRelacionado> tbPainelModificacaoRelacionadoes { get; set; }
        public virtual ICollection<tbPainelModificacaoRelacionado> tbPainelModificacaoRelacionadoes1 { get; set; }
        public virtual tbPainelModificacaoOperacao tbPainelModificacaoOperacao { get; set; }
        public virtual ICollection<tbPedidoProduto> tbPedidoProdutoes { get; set; }
        public virtual ICollection<tbPainelModificacaoProduto> tbPainelModificacaoProdutoes { get; set; }
        public virtual ICollection<tbProdutoPainelModificacao> tbProdutoPainelModificacaos { get; set; }
    }
}
