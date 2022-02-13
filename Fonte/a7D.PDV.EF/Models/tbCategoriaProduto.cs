using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public class tbCategoriaProduto : IERP, IERPSync
    {
        public tbCategoriaProduto()
        {
            this.tbPainelModificacaoCategorias = new List<tbPainelModificacaoCategoria>();
            this.tbProdutoCategoriaProdutoes = new List<tbProdutoCategoriaProduto>();
        }

        public int IDCategoriaProduto { get; set; }
        public string CodigoERP { get; set; }
        public string Nome { get; set; }
        public int? Cor { get; set; }
        public int? IDImagem { get; set; }
        public string TAG { get; set; }
        public int? IDTipoCategoria { get; set; } //Mesa/Comanda, Delivery, Insumo, Pré-Preparo
        public bool Excluido { get; set; }
        public bool Disponibilidade { get; set; }
        public bool? Ativo { get; set; }
        public DateTime DtUltimaAlteracao { get; set; }
        public DateTime DtAlteracaoDisponibilidade { get; set; }

        public virtual ICollection<tbPainelModificacaoCategoria> tbPainelModificacaoCategorias { get; set; }
        public virtual ICollection<tbProdutoCategoriaProduto> tbProdutoCategoriaProdutoes { get; set; }

        public int myID() => IDCategoriaProduto;

        public bool RequerAlteracaoERP(DateTime dtSync) => DtUltimaAlteracao > dtSync;

        public override string ToString() => $"{IDCategoriaProduto}: {Nome}";
    }
}
