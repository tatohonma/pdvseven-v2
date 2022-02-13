using System.Runtime.Serialization;

namespace a7D.PDV.Integracao.API2.Model
{
    [DataContract]
    public class ConfigKeyPar
    {
        [DataMember()]
        public string chave { get; set; }

        [DataMember()]
        public string valor { get; set; }

        public ConfigKeyPar(string c, string v)
        {
            chave = c;
            valor = v;
        }
    }
}
