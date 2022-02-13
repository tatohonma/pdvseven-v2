using a7D.PDV.Model;
using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbConfiguracaoBD
    {
        public int? IDConfiguracaoBD { get; set; }
        public int? IDTipoPDV { get; set; }
        public int? IDPDV { get; set; }
        public string Chave { get; set; }
        public string Valor { get; set; }
        public string ValoresAceitos { get; set; }
        public bool? Obrigatorio { get; set; }
        public string Titulo { get; set; }
        public virtual PDVInformation PDV { get; set; }
        public virtual TipoPDVInformation TipoPDV { get; set; }

        public override string ToString()
        {
            return $"{IDTipoPDV}:{Chave}={Valor} - {Titulo}";
        }
    }
}
