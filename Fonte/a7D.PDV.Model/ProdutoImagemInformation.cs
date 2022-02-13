using a7D.Fmk.CRUD.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace a7D.PDV.Model
{
    [Serializable]
    [CRUDClassDAL("tbProdutoImagem")]
    public class ProdutoImagemInformation
    {
        [CRUDParameterDAL(true, "IDProdutoImagem")]
        public int? IDProdutoImagem { get; set; }

        [CRUDParameterDAL(false, "IDProduto", "IDProduto")]
        public ProdutoInformation Produto { get; set; }

        [CRUDParameterDAL(false, "IDImagem", "IDImagem")]
        public ImagemInformation Imagem { get; set; }
    }
}
