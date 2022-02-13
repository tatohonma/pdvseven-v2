using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace a7D.PDV.Integracao.API.Responses
{
    class ListarProdutosCategoriaProduto : BaseResponse
    {
        public int[] produtos { get; set; }
        public ProdutoCategoriaProduto[] produtosCategoriaProduto { get; set; }

        public class ProdutoCategoriaProduto
        {
            public int idProdutoCategoriaProduto { get; set; }
            public int idProduto { get; set; }
            public int idCategoriaProduto { get; set; }
        }
    }
}