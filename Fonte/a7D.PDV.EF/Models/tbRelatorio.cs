using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbRelatorio
    {
        public int IDRelatorio { get; set; }
        public int IDTipoRelatorio { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string QuerySQL { get; set; }
        public string Parametros { get; set; }
        public int Ordem { get; set; }
        public string Totalizador { get; set; }
        public virtual tbTipoRelatorio tbTipoRelatorio { get; set; }
    }
}
