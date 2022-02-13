using a7D.Fmk.CRUD.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Model
{
    [Serializable]
    [CRUDClassDAL("tbMovimentacaoProdutos")]
    public class MovimentacaoProdutosInformation
    {
        [CRUDParameterDAL(true, "IDMovimentacaoProdutos")]
        public int? IDMovimentacaoProdutos { get; set; }

        [CRUDParameterDAL(false, "IDMovimentacao", "IDMovimentacao")]
        public MovimentacaoInformation Movimentacao { get; set; }

        [CRUDParameterDAL(false, "IDProduto", "IDProduto")]
        public ProdutoInformation Produto { get; set; }

        [CRUDParameterDAL(false, "IDUnidade", "IDUnidade")]
        public UnidadeInformation Unidade { get; set; }

        [CRUDParameterDAL(false, "Quantidade")]
        public decimal? Quantidade { get; set; }

        public decimal? QuantidadeRelativa
        {
            get
            {
                if (Movimentacao == null || Movimentacao.TipoMovimentacao == null || Movimentacao.TipoMovimentacao.IDTipoMovimentacao.HasValue == false)
                    return Quantidade;
                else if (Quantidade.HasValue)
                {
                    var tipoMovimentacao = Movimentacao.TipoMovimentacao.IDTipoMovimentacao.Value;
                    if (tipoMovimentacao == 2)
                        return (Quantidade * -1);
                    else
                        return Quantidade;
                }
                else
                    return Quantidade;
            }
        }

        public StatusModel StatusModel { get; set; }
    }
}
