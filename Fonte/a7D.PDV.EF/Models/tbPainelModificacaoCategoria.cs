using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbPainelModificacaoCategoria
    {
        public int IDPainelModificacaoCategoria { get; set; }
        public int IDPainelModificacao { get; set; }
        public int IDCategoriaProduto { get; set; }
        public int Ordem { get; set; }
        public virtual tbCategoriaProduto tbCategoriaProduto { get; set; }
        public virtual tbPainelModificacao tbPainelModificacao { get; set; }
    }
}
