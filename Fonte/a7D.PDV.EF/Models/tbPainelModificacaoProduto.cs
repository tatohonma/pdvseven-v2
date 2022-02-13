using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbPainelModificacaoProduto
    {
        public int IDPainelModificacaoProduto { get; set; }
        public int IDPainelModificacao { get; set; }
        public int IDProduto { get; set; }
        public Nullable<bool> IgnorarValorItem { get; set; }
        public int Ordem { get; set; }
        public virtual tbPainelModificacao tbPainelModificacao { get; set; }
        public virtual tbProduto tbProduto { get; set; }
    }
}
