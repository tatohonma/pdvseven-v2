using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbMapAreaImpressaoProduto
    {
        public int IDMapAreaImpressaoProduto { get; set; }
        public int IDAreaImpressao { get; set; }
        public int IDProduto { get; set; }
        public virtual tbAreaImpressao tbAreaImpressao { get; set; }
        public virtual tbProduto tbProduto { get; set; }
    }
}
