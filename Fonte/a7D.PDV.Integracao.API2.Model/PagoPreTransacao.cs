using System.Runtime.Serialization;

namespace a7D.PDV.Integracao.API2.Model
{
    [DataContract]
    public class PagoPreTransacao
    {
        [DataMember(EmitDefaultValue = false)]
        public string document;
        [DataMember(EmitDefaultValue = false)]
        public string id;
        [DataMember(EmitDefaultValue = false)]
        public string pdv;
        [DataMember(EmitDefaultValue = false)]
        public string serial;
        [DataMember(EmitDefaultValue = false)]
        public string dateTime;
        [DataMember(EmitDefaultValue = false)]
        public PagoPreField[] fields;
        [DataMember(EmitDefaultValue = false)]
        public string fieldId;
        [DataMember(EmitDefaultValue = false)]
        public int value;
    }

    [DataContract]
    public class PagoPreField
    {
        [DataMember(EmitDefaultValue = false)]
        public string fieldId;
        [DataMember(EmitDefaultValue = false)]
        public string value;
    }
}