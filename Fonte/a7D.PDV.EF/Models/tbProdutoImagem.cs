using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbProdutoImagem
    {
        public int IDProdutoImagem { get; set; }
        public int IDProduto { get; set; }
        public int IDImagem { get; set; }
        public virtual tbImagem tbImagem { get; set; }
        public virtual tbProduto tbProduto { get; set; }
    }
}
