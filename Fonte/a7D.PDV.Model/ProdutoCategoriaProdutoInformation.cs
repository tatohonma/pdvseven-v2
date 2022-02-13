using a7D.Fmk.CRUD.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace a7D.PDV.Model
{
    [Serializable]
    [CRUDClassDAL("tbProdutoCategoriaProduto")]
    public class ProdutoCategoriaProdutoInformation
    {
        [CRUDParameterDAL(true, "IDProdutoCategoriaProduto")]
        public int? IDProdutoCategoriaProduto { get; set; }

        [CRUDParameterDAL(false, "IDProduto", "IDProduto")]
        public ProdutoInformation Produto { get; set; }

        [CRUDParameterDAL(false, "IDCategoriaProduto", "IDCategoriaProduto")]
        public CategoriaProdutoInformation CategoriaProduto { get; set; }

        public StatusModel StatusModel { get; set; }

        public override string ToString()
        {
            return $"{CategoriaProduto?.ToString()} - {Produto?.ToString()} ";
        }
    }
}
