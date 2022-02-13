using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;

namespace a7D.PDV.SATService.Controllers
{
    [DataContract]
    public class EnvApi
    {
        [DataMember]
        public int NumeroSessao { get; set; }

        [DataMember]
        public string Chave { get; set; }

        [DataMember]
        public string DadosCancelamento { get; set; }

        public override string ToString()
        {
            return JToken.FromObject(this).ToString();
        }
    }
}