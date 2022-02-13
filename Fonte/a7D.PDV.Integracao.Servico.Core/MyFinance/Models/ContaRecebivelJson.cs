using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.Servico.Core.MyFinance.Models
{
    public class ContaRecebivelJson
    {
        [JsonProperty(PropertyName = "sale_account")]
        public ContaRecebivel ContaRecebivel { get; set; }
    }
}
