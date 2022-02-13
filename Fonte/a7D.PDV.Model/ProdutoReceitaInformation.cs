using a7D.Fmk.CRUD.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Model
{
    [Serializable]
    [CRUDClassDAL("tbProdutoReceita")]
    public class ProdutoReceitaInformation
    {
        [CRUDParameterDAL(true, "IDProdutoReceita")]
        public int? IDProdutoReceita { get; set; }

        [CRUDParameterDAL(false, "IDProduto", "IDProduto")]
        public ProdutoInformation Produto { get; set; }

        [CRUDParameterDAL(false, "IDProduto_ingrediente", "IDProduto")]
        public ProdutoInformation ProdutoIngrediente { get; set; }

        [CRUDParameterDAL(false, "IDUnidade", "IDUnidade")]
        public UnidadeInformation Unidade { get; set; }

        [CRUDParameterDAL(false, "Quantidade")]
        public decimal? Quantidade { get; set; }

        public StatusModel StatusModel { get; set; }
    }
}
