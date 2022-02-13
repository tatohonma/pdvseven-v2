using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbInventarioProduto
    {
        public int IDInventarioProdutos { get; set; }
        public int IDInventario { get; set; }
        public int IDProduto { get; set; }
        public int IDUnidade { get; set; }
        public decimal Quantidade { get; set; }
        public decimal QuantidadeAnterior { get; set; }
        public virtual tbInventario tbInventario { get; set; }
        public virtual tbProduto tbProduto { get; set; }
        public virtual tbUnidade tbUnidade { get; set; }
    }
}
