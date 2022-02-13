using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbEntradaSaida
    {
        public int IDEntradaSaida { get; set; }
        public string GUID_Origem { get; set; }
        public int IDProduto { get; set; }
        public bool Entrada { get; set; }
        public decimal Quantidade { get; set; }
        public System.DateTime Data { get; set; }
        public virtual tbProduto tbProduto { get; set; }
    }
}
