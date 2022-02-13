using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbConversaoUnidade
    {
        public int IDConversaoUnidade { get; set; }
        public int IDUnidade_de { get; set; }
        public int IDUnidade_para { get; set; }
        public decimal Divisao { get; set; }
        public decimal Multiplicacao { get; set; }
        public virtual tbUnidade tbUnidade { get; set; }
        public virtual tbUnidade tbUnidade1 { get; set; }
    }
}
