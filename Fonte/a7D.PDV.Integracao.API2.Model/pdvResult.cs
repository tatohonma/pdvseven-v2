using System.Runtime.Serialization;

namespace a7D.PDV.Integracao.API2.Model
{
    [DataContract]
    public class pdvResult : ErroResult
    {
        [DataMember(EmitDefaultValue = false)]
        public int idPDV;

        [DataMember(EmitDefaultValue = false)]
        public string nome;

        [DataMember(EmitDefaultValue = false)]
        public string versaoWS;
    }
}
