using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbClassificacaoFiscal
    {
        public tbClassificacaoFiscal()
        {
            this.tbProdutoes = new List<tbProduto>();
        }

        public int IDClassificacaoFiscal { get; set; }
        public int IDTipoTributacao { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string NCM { get; set; }
        public string CEST { get; set; }
        public decimal? IOF { get; set; }
        public decimal? IPI { get; set; }
        public decimal? PISPASEP { get; set; }
        public decimal? CIDE { get; set; }
        public decimal? COFINS { get; set; }
        public decimal? ICMS { get; set; }
        public decimal? ISS { get; set; }
        public virtual tbTipoTributacao tbTipoTributacao { get; set; }
        public virtual ICollection<tbProduto> tbProdutoes { get; set; }
    }
}
