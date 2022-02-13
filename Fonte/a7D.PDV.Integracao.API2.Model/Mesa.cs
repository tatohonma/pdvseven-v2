using System.Runtime.Serialization;

namespace a7D.PDV.Integracao.API2.Model
{
    /// <summary>
    /// Pedido
    /// </summary>
    [DataContract]
    public partial class Mesa
    {
        /// <summary>
        /// status da mesa
        /// </summary>
        [DataMember(Name = "status", EmitDefaultValue = false)]
        public int? status { get; set; }
        /// <summary>
        /// numero do pedido aberto
        /// </summary>
        [DataMember(Name = "idPedidoAberto", EmitDefaultValue = false)]
        public int? idPedidoAberto { get; set; }
        /// <summary>
        /// valor total da mesa atualmente aberta
        /// </summary>
        [DataMember(Name = "ValorTotal", EmitDefaultValue = false)]
        public decimal? ValorTotal { get; set; }

        /// <summary>
        /// Total pago até o momento (parcial)
        /// </summary>
        [DataMember(Name = "ValorPago", EmitDefaultValue = false)]
        public decimal? ValorPago { get; set; }

        /// <summary>
        /// Valor dos Serviços
        /// </summary>
        [DataMember(Name = "ValorServico", EmitDefaultValue = false)]
        public decimal? ValorServico { get; set; }

        [DataMember(Name = "ClienteNome", EmitDefaultValue = false)]
        public string ClienteNome { get; set; }

        [DataMember(Name = "ClienteDocumento", EmitDefaultValue = false)]
        public string ClienteDocumento { get; set; }

        [DataMember(Name = "DocumentoFiscal", EmitDefaultValue = false)]
        public string DocumentoFiscal { get; set; }
    }
}
