using System.Runtime.Serialization;

namespace a7D.PDV.Integracao.API2.Model
{
    [DataContract]
    public class PagoPreRetorno
    {
        [DataMember(EmitDefaultValue = false)]
        public string id;
        [DataMember(EmitDefaultValue = false)]
        public string pdv;
        [DataMember(EmitDefaultValue = false)]
        public int amount;
        [DataMember(EmitDefaultValue = false)]
        public bool multiPayment;
        [DataMember(EmitDefaultValue = false)]
        public bool cashPayment;
        [DataMember(EmitDefaultValue = false)]
        public int condition;
        [DataMember(EmitDefaultValue = true)]
        public int paymentMethodType;
        [DataMember(EmitDefaultValue = true)]
        public int paymentMode;
        [DataMember(EmitDefaultValue = true)]
        public int installmentMode;
        [DataMember(EmitDefaultValue = true)]
        public int installmentsNumber;
        [DataMember(EmitDefaultValue = true)]
        public string displayMessage;
        [DataMember(EmitDefaultValue = true)]
        public string printBefore;
        [DataMember(EmitDefaultValue = true)]
        public string printAfter;
        [DataMember(EmitDefaultValue = true)]
        public bool printSummary;
        [DataMember(EmitDefaultValue = false)]
        public bool endOperation;
    }
}