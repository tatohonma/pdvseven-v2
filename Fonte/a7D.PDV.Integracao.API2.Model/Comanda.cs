using System;
using System.Runtime.Serialization;
using System.Text;

namespace a7D.PDV.Integracao.API2.Model
{
    /// <summary>
    /// Pedido
    /// </summary>
    [DataContract]
    public partial class Comanda : ErroResult
    {
        /// <summary>
        /// status da comanda
        /// </summary>
        [DataMember(Name = "status", EmitDefaultValue = false)]
        public int? status { get; set; }

        /// <summary>
        /// numero do pedido aberto
        /// </summary>
        [DataMember(Name = "idPedidoAberto", EmitDefaultValue = false)]
        public int? idPedidoAberto { get; set; }

        [DataMember(Name = "ValorTotal", EmitDefaultValue = false)]
        public decimal? ValorTotal { get; set; }

        /// <summary>
        /// valor total da comanda atualmente aberta
        /// </summary>
        [DataMember(Name = "Numero", EmitDefaultValue = false)]
        public int? Numero { get; set; }

        /// <summary>
        /// valor da TAG (Código) já convertido em HEX
        /// </summary>
        [DataMember(Name = "TAGHEX", EmitDefaultValue = false)]
        public string TAGHEX { get; set; }

        /// <summary>
        /// valor da TAG (Código) original em DECIMAL
        /// </summary>
        [DataMember(Name = "TAGDEC", EmitDefaultValue = false)]
        public long? TAGDEC { get; set; }

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

        [DataMember(Name = "ReferenciaLocalizacao", EmitDefaultValue = false)]
        public string ReferenciaLocalizacao { get; set; }

        /// <summary>
        /// Nome do cliente
        /// </summary>
        [DataMember(Name = "ClienteNome", EmitDefaultValue = false)]
        [Obsolete]
        public string ClienteNome { get; set; }

        /// <summary>
        /// CPF do cliente
        /// </summary>
        [DataMember(Name = "ClienteDocumento", EmitDefaultValue = false)]
        [Obsolete]
        public string ClienteDocumento { get; set; }


        [DataMember(Name = "Cliente", EmitDefaultValue = false)]
        public Cliente Cliente { get; set; }

        /// <summary>
        /// Documento Fiscal
        /// </summary>
        [DataMember(Name = "DocumentoFiscal", EmitDefaultValue = false)]
        public string DocumentoFiscal { get; set; }

        /// <summary>
        /// Limite de valor para novos pedidos
        /// </summary>
        [DataMember(Name = "Limite", EmitDefaultValue = false)]
        public decimal? Limite { get; set; }

        /// <summary>
        /// Credito disponivel para compra pré paga
        /// </summary>
        [DataMember(Name = "Credito", EmitDefaultValue = false)]
        public decimal? Credito { get; set; }

        [DataMember(Name = "ValorDisponivel", EmitDefaultValue = false)]
        public decimal? ValorDisponivel { get => Credito ?? Limite; set { } }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("{\n");
            sb.Append("  status: ").Append(status).Append("\n");
            sb.Append("  idPedidoAberto: ").Append(idPedidoAberto).Append("\n");
            sb.Append("  ValorTotal: ").Append(ValorTotal).Append("\n");
            sb.Append("  ValorPago: ").Append(ValorPago).Append("\n");
            sb.Append("  ValorServico: ").Append(ValorServico).Append("\n");
            sb.Append("  ClienteNome: ").Append(ClienteNome ?? "null").Append("\n");
            sb.Append("  DocumentoFiscal: ").Append(DocumentoFiscal ?? "null").Append("\n");

            if (Credito != null)
                sb.Append("  Credito: ").Append(Credito).Append("\n");

            if (Limite != null)
                sb.Append("  Limite: ").Append(Limite).Append("\n");

            sb.Append("}\n");
            return sb.ToString();
        }
    }
}