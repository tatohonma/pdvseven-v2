using a7D.Fmk.CRUD.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbInventarioProdutos")]
    public class InventarioProdutosInformation
    {
        [CRUDParameterDAL(true, "IDInventarioProdutos")]
        public int? IDInventarioProdutos { get; set; }

        [CRUDParameterDAL(false, "IDInventario", "IDInventario")]
        public InventarioInformation Inventario { get; set; }

        [CRUDParameterDAL(false, "IDProduto", "IDProduto")]
        public ProdutoInformation Produto { get; set; }

        [CRUDParameterDAL(false, "IDUnidade", "IDUnidade")]
        public UnidadeInformation Unidade { get; set; }

        [CRUDParameterDAL(false, "Quantidade")]
        public decimal? Quantidade { get; set; }

        [CRUDParameterDAL(false, "QuantidadeAnterior")]
        public decimal? QuantidadeAnterior { get; set; }

        public StatusModel StatusModel { get; set; }
    }
}
