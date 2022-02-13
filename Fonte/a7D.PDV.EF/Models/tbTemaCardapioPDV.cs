using a7D.PDV.Model;
using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbTemaCardapioPDV
    {
        public int IDTemaCardapioPDV { get; set; }
        public int IDTemaCardapio { get; set; }
        public Nullable<int> IDPDV { get; set; }
        public bool Ativo { get; set; }
        public System.DateTime DtUltimaAlteracao { get; set; }
        public virtual PDVInformation tbPDV { get; set; }
        public virtual tbTemaCardapio tbTemaCardapio { get; set; }
    }
}
