using System.Runtime.Serialization;

namespace a7D.PDV.Integracao.API2.Model
{
    [DataContract]
    public class Entrada : ErroResult
    {
        [DataMember(EmitDefaultValue = false)]
        public int idEntrada;

        [DataMember(EmitDefaultValue = false)]
        public string Nome;

        [DataMember(EmitDefaultValue = false)]
        public decimal ValorEntrada { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public decimal ValorConsumacaoMinima;
    }
}
