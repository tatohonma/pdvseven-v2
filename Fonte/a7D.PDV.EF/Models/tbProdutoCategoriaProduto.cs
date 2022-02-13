using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbProdutoCategoriaProduto
    {
        public int IDProdutoCategoriaProduto { get; set; }
        public int IDProduto { get; set; }
        public int IDCategoriaProduto { get; set; }
        public virtual tbCategoriaProduto tbCategoriaProduto { get; set; }
        public virtual tbProduto tbProduto { get; set; }
    }
}
