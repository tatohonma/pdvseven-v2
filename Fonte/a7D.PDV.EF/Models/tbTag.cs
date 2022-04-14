using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbTag
    {
        public int IDTag { get; set; }
        public string GUIDIdentificacao { get; set; }
        public string Chave { get; set; }
        public string Valor { get; set; }
        public DateTime DtInclusao { get; set; }
    }
}
