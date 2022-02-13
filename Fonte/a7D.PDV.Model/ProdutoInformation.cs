using a7D.Fmk.CRUD.DAL;
using a7D.PDV.EF;
using System;
using System.Collections.Generic;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbProduto")]
    [Serializable]
    public class ProdutoInformation : IERP
    {
        public static int IDProdutoNaoCadastrado = 1;
        public static int IDProdutoEntrada = 2;
        public static int IDProdutoEntracaCM = 3;
        public static int IDProdutoServico = 4;

        [CRUDParameterDAL(true, "IDProduto")]
        public Int32? IDProduto { get; set; }

        [CRUDParameterDAL(false, "IDTipoProduto", "IDTipoProduto")]
        public TipoProdutoInformation TipoProduto { get; set; }

        [CRUDParameterDAL(false, "Codigo")]
        public String Codigo { get; set; }

        [CRUDParameterDAL(false, "CodigoERP")]
        public String CodigoERP { get; set; }

        [CRUDParameterDAL(false, "Nome")]
        public String Nome { get; set; }

        [CRUDParameterDAL(false, "Descricao")]
        public String Descricao { get; set; }

        [CRUDParameterDAL(false, "ValorUnitario")]
        public Decimal? ValorUnitario { get; set; }

        [CRUDParameterDAL(false, "ValorUnitario2")]
        public Decimal? ValorUnitario2 { get; set; }

        [CRUDParameterDAL(false, "ValorUnitario3")]
        public Decimal? ValorUnitario3 { get; set; }

        [CRUDParameterDAL(false, "CodigoAliquota")]
        public String CodigoAliquota { get; set; }

        [CRUDParameterDAL(false, "Ativo")]
        public Boolean? Ativo { get; set; }

        [CRUDParameterDAL(false, "AssistenteModificacoes")]
        public Boolean? AssistenteModificacoes { get; set; }

        [CRUDParameterDAL(false, "DtUltimaAlteracao")]
        public DateTime? DtUltimaAlteracao { get; set; }

        [CRUDParameterDAL(false, "Disponibilidade")]
        public Boolean? Disponibilidade { get; set; }

        [CRUDParameterDAL(false, "DtAlteracaoDisponibilidade")]
        public DateTime? DtAlteracaoDisponibilidade { get; set; }

        [CRUDParameterDAL(false, "Excluido")]
        public Boolean? Excluido { get; set; }

        [CRUDParameterDAL(false, "IDClassificacaoFiscal", "IDClassificacaoFiscal")]
        public ClassificacaoFiscalInformation ClassificacaoFiscal { get; set; }

        [CRUDParameterDAL(false, "IDUnidade", "IDUnidade")]
        public UnidadeInformation Unidade { get; set; }

        [CRUDParameterDAL(false, "cEAN")]
        public String cEAN { get; set; }

        [CRUDParameterDAL(false, "ControlarEstoque")]
        public bool? ControlarEstoque { get; set; }

        [CRUDParameterDAL(false, "EstoqueMinimo")]
        public decimal? EstoqueMinimo { get; set; }

        [CRUDParameterDAL(false, "EstoqueIdeal")]
        public decimal? EstoqueIdeal { get; set; }

        [CRUDParameterDAL(false, "UtilizarBalanca")]
        public bool? UtilizarBalanca { get; set; }

        public override int GetHashCode()
        {
            var hash = 13;
            return (hash * 7) + (IDProduto.HasValue ? IDProduto.Value.GetHashCode() : (string.IsNullOrEmpty(Nome) == false ? Nome.GetHashCode() : 0));
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            ProdutoInformation pi = (ProdutoInformation)obj;

            return GetHashCode() == pi.GetHashCode();
        }

        public List<ProdutoPainelModificacaoInformation> ListaPainelModificacao { get; set; }

        public List<ProdutoCategoriaProdutoInformation> ListaProdutoCategoria { get; set; }

        public List<ProdutoTraducaoInformation> ListaProdutoTraducao { get; set; }

        public List<ProdutoReceitaInformation> ListaProdutoReceita { get; set; }

        public override string ToString()
        {
            return $"{IDProduto ?? 0}: {Nome}";
        }
    }
}
