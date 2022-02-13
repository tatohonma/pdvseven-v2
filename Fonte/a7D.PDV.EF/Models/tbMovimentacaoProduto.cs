using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbMovimentacaoProduto
    {
        public int IDMovimentacaoProdutos { get; set; }
        public int IDMovimentacao { get; set; }
        public int IDProduto { get; set; }
        public int IDUnidade { get; set; }
        public decimal Quantidade { get; set; }
        public virtual tbMovimentacao tbMovimentacao { get; set; }
        public virtual tbProduto tbProduto { get; set; }
        public virtual tbUnidade tbUnidade { get; set; }
    }
}
