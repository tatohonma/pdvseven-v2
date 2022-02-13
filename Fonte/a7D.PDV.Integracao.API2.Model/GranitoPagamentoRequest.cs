using System;
using System.Runtime.Serialization;

namespace a7D.PDV.Integracao.API2.Model
{
    [DataContract]
    public class GranitoPagamentoRequest
    {
        [DataMember(Name = "merchantId", EmitDefaultValue = false)]
        public int MerchantId { get; set; }
        [DataMember(Name = "pdv", EmitDefaultValue = false)]
        public int PDVId { get; set; }
        [DataMember(Name = "serial", EmitDefaultValue = false)]
        public string serial { get; set; }
        [DataMember(Name = "operationType", EmitDefaultValue = false)]
        public int OperationType { get; set; }
        [DataMember(Name = "transactionId", EmitDefaultValue = false)]
        public int TransactionId { get; set; }
        [DataMember(Name = "cardNumber", EmitDefaultValue = false)]
        public double CardNumber { get; set; }
        [DataMember(Name = "brandCode", EmitDefaultValue = false)]
        public int BrandCode { get; set; }
        [DataMember(Name = "networkCode", EmitDefaultValue = false)]
        public int NetworkCode { get; set; }
        [DataMember(Name = "paymentMethodType", EmitDefaultValue = false)]
        public int PaymentMethodType { get; set; }
        [DataMember(Name = "paymentMode", EmitDefaultValue = false)]
        public int PaymentMode { get; set; }
        [DataMember(Name = "installmentMode", EmitDefaultValue = false)]
        public int InstallmentMode { get; set; }
        [DataMember(Name = "installmentsNumber", EmitDefaultValue = false)]
        public int InstallmentsNumber { get; set; }
        [DataMember(Name = "amount", EmitDefaultValue = false)]
        public string Amount { get; set; }
        [DataMember(Name = "proofOfSale", EmitDefaultValue = false)]
        public string ProofOfSale { get; set; }
        [DataMember(Name = "transactionCode", EmitDefaultValue = false)]
        public string TransactionCode { get; set; }
        [DataMember(Name = "transactionDate", EmitDefaultValue = false)]
        public string TransactionDate { get; set; }
        [DataMember(Name = "fields", EmitDefaultValue = false)]
        public GranitoFieldValue[] Fields { get; set; }
    }
}
