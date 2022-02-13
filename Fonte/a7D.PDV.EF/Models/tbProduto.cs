using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public class tbProduto : IERP, IERPSync
    {
        public tbProduto()
        {
            this.tbEntradaSaidas = new List<tbEntradaSaida>();
            this.tbInventarioProdutos = new List<tbInventarioProduto>();
            this.tbMapAreaImpressaoProdutoes = new List<tbMapAreaImpressaoProduto>();
            this.tbMovimentacaoProdutos = new List<tbMovimentacaoProduto>();
            this.tbPainelModificacaoProdutoes = new List<tbPainelModificacaoProduto>();
            this.tbPedidoProdutoes = new List<tbPedidoProduto>();
            this.tbProdutoTraducaos = new List<tbProdutoTraducao>();
            this.tbProdutoCategoriaProdutoes = new List<tbProdutoCategoriaProduto>();
            this.tbProdutoImagems = new List<tbProdutoImagem>();
            this.tbProdutoPainelModificacaos = new List<tbProdutoPainelModificacao>();
            this.tbProdutoReceitas = new List<tbProdutoReceita>();
            this.tbProdutoReceitas1 = new List<tbProdutoReceita>();
        }

        public int IDProduto { get; set; }
        public int IDTipoProduto { get; set; }
        public string Codigo { get; set; }
        public string CodigoERP { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string TAG { get; set; }
        public int? Cor { get; set; }
        public bool? IsentoTaxaServico { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal? ValorUnitario2 { get; set; }
        public decimal? ValorUnitario3 { get; set; }
        public decimal? ValorUltimaCompra { get; set; }
        public DateTime? DataUltimaCompra { get; set; }
        public string FrequenciaCompra { get; set; }
        public string CodigoAliquota { get; set; }
        public bool Ativo { get; set; }
        public bool Disponibilidade { get; set; }
        public bool? DisponibilidadeModificacao { get; set; }
        public DateTime DtAlteracaoDisponibilidade { get; set; }
        public DateTime DtUltimaAlteracao { get; set; }
        public bool Excluido { get; set; }
        public int? IDClassificacaoFiscal { get; set; }
        public int? IDUnidade { get; set; }
        public string cEAN { get; set; }
        public bool ControlarEstoque { get; set; }
        public decimal? EstoqueMinimo { get; set; }
        public decimal? EstoqueIdeal { get; set; }
        public decimal? EstoqueAtual { get; set; }
        public DateTime? EstoqueData { get; set; }
        public bool UtilizarBalanca { get; set; }
        public bool? AssistenteModificacoes { get; set; }
        public virtual tbClassificacaoFiscal tbClassificacaoFiscal { get; set; }
        public virtual ICollection<tbEntradaSaida> tbEntradaSaidas { get; set; }
        public virtual ICollection<tbInventarioProduto> tbInventarioProdutos { get; set; }
        public virtual ICollection<tbMapAreaImpressaoProduto> tbMapAreaImpressaoProdutoes { get; set; }
        public virtual ICollection<tbMovimentacaoProduto> tbMovimentacaoProdutos { get; set; }
        public virtual ICollection<tbPainelModificacaoProduto> tbPainelModificacaoProdutoes { get; set; }
        public virtual ICollection<tbPedidoProduto> tbPedidoProdutoes { get; set; }
        public virtual ICollection<tbProdutoTraducao> tbProdutoTraducaos { get; set; }
        public virtual tbTipoProduto tbTipoProduto { get; set; }
        public virtual tbUnidade tbUnidade { get; set; }
        public virtual ICollection<tbProdutoCategoriaProduto> tbProdutoCategoriaProdutoes { get; set; }
        public virtual ICollection<tbProdutoImagem> tbProdutoImagems { get; set; }
        public virtual ICollection<tbProdutoPainelModificacao> tbProdutoPainelModificacaos { get; set; }
        public virtual ICollection<tbProdutoReceita> tbProdutoReceitas { get; set; }
        public virtual ICollection<tbProdutoReceita> tbProdutoReceitas1 { get; set; }

        public bool RequerAlteracaoERP(DateTime dtSync) 
            => DtUltimaAlteracao > dtSync;

        public int myID() => IDProduto;

        public override string ToString() => $"{IDProduto}: {Nome}";
    }
}
