using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbProdutoReceita
    {
        public int IDProdutoReceita { get; set; }
        public int IDProduto { get; set; }
        public int IDProduto_ingrediente { get; set; }
        public int IDUnidade { get; set; }
        public decimal Quantidade { get; set; }
        public virtual tbProduto tbProduto { get; set; }
        public virtual tbProduto tbProduto1 { get; set; }
        public virtual tbUnidade tbUnidade { get; set; }
    }
}
