using System.Runtime.Serialization;

namespace a7D.PDV.Fiscal.Comunicacao.SAT
{
    [DataContract]
    public class EnvApiCancelamento
    {
        [DataMember]
        public int NumeroSessao { get; set; }

        [DataMember]
        public string Chave { get; set; }

        [DataMember]
        public string DadosCancelamento { get; set; }
    }
}