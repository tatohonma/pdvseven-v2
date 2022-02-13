using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbAreaImpressao
    {
        public tbAreaImpressao()
        {
            this.tbMapAreaImpressaoProdutoes = new List<tbMapAreaImpressaoProduto>();
            this.tbOrdemImpressaos = new List<tbOrdemImpressao>();
        }

        public int IDAreaImpressao { get; set; }
        public string Nome { get; set; }
        public string NomeImpressora { get; set; } // Ou o nome do PDV + Impressora Local
        public int? IDPDV { get; set; } // Impressora Windows, Caixa
        public int IDTipoAreaImpressao { get; set; }
        public virtual tbTipoAreaImpressao tbTipoAreaImpressao { get; set; }
        public virtual ICollection<tbMapAreaImpressaoProduto> tbMapAreaImpressaoProdutoes { get; set; }
        public virtual ICollection<tbOrdemImpressao> tbOrdemImpressaos { get; set; }

        public override string ToString()
        {
            return $"{IDAreaImpressao}: {Nome}";
        }
    }
}
