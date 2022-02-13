using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbPainelModificacaoOperacao
    {
        public tbPainelModificacaoOperacao()
        {
            this.tbPainelModificacaos = new List<tbPainelModificacao>();
        }

        public int IDPainelModificacaoOperacao { get; set; }
        public string Nome { get; set; }
        public virtual ICollection<tbPainelModificacao> tbPainelModificacaos { get; set; }
    }
}
