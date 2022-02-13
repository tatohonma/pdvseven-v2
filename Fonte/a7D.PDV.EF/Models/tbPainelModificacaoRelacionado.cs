using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbPainelModificacaoRelacionado
    {
        public int IDPainelModificacaoRelacionado { get; set; }
        public int IDPainelModificacao1 { get; set; }
        public int IDPainelModificacao2 { get; set; }
        public Nullable<bool> IgnorarValorItem { get; set; }
        public virtual tbPainelModificacao tbPainelModificacao { get; set; }
        public virtual tbPainelModificacao tbPainelModificacao1 { get; set; }
    }
}
