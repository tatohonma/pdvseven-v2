using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.PixConta.Model
{
    public class InvoiceInformation
    {
        public string id { get; set; }
        public string status { get; set; }
        public int total_cents { get; set; }
    }
}
