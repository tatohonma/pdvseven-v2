using System;
using System.Runtime.Serialization;

namespace a7D.PDV.Integracao.API2.Model
{
    /// <summary>
    /// Tipo de entrada utilizada no pedido
    /// </summary>
    [DataContract]
    public partial class TipoEntrada
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TipoEntrada" /> class.
        /// </summary>
        /// <param name="IDTipoEntrada">Chave primária deste tipo de entrada.</param>
        /// <param name="Nome">Nome deste tipo de entrada.</param>
        /// <param name="ValorEntrada">Valor deste tipo de entrada.</param>
        /// <param name="ValorConsumacaoMinima">Valor da consumação mínima deste tipo de entrada.</param>
        public TipoEntrada(int? IDTipoEntrada = default(int?), string Nome = default(string), decimal? ValorEntrada = default(decimal?), decimal? ValorConsumacaoMinima = default(decimal?))
        {
            this.IDTipoEntrada = IDTipoEntrada;
            this.Nome = Nome;
            this.ValorEntrada = ValorEntrada;
            this.ValorConsumacaoMinima = ValorConsumacaoMinima;
        }

     

        /// <summary>
        /// Chave primária deste tipo de entrada
        /// </summary>
        /// <value>Chave primária deste tipo de entrada</value>
        [DataMember(Name = "IDTipoEntrada", EmitDefaultValue = false)]
        public int? IDTipoEntrada { get; set; }
        /// <summary>
        /// Nome deste tipo de entrada
        /// </summary>
        /// <value>Nome deste tipo de entrada</value>
        [DataMember(Name = "Nome", EmitDefaultValue = false)]
        public string Nome { get; set; }
        /// <summary>
        /// Valor deste tipo de entrada
        /// </summary>
        /// <value>Valor deste tipo de entrada</value>
        [DataMember(Name = "ValorEntrada", EmitDefaultValue = false)]
        public decimal? ValorEntrada { get; set; }
        /// <summary>
        /// Valor da consumação mínima deste tipo de entrada
        /// </summary>
        /// <value>Valor da consumação mínima deste tipo de entrada</value>
        [DataMember(Name = "ValorConsumacaoMinima", EmitDefaultValue = false)]
        public decimal? ValorConsumacaoMinima { get; set; }
    }
}
