using System.Runtime.Serialization;

namespace a7D.PDV.Integracao.API2.Model
{
    /// <summary>
    /// ProdutoPainelModificacao
    /// </summary>
    [DataContract]
    public partial class ProdutoPainelModificacao
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProdutoPainelModificacao" /> class.
        /// </summary>
        /// <param name="IDProduto">ID do Produto que pertence ao painel de modificação.</param>
        /// <param name="Valor">Valor adicional desta modificação.</param>
        /// <param name="Ordem">Ordem em que este produto deve ser mostrado no painel de modificação</param>
        public ProdutoPainelModificacao(int? IDProduto = default(int?), decimal? Valor = default(decimal?), int? Ordem = default(int?))
        {
            this.IDProduto = IDProduto;
            this.Valor = Valor;
            this.Ordem = Ordem;
        }
        
        /// <summary>
        /// ID do Produto que pertence ao painel de modificação
        /// </summary>
        /// <value>ID do Produto que pertence ao painel de modificação</value>
        [DataMember(Name="IDProduto", EmitDefaultValue=false)]
        public int? IDProduto { get; set; }
        /// <summary>
        /// Valor adicional desta modificação
        /// </summary>
        /// <value>Valor adicional desta modificação</value>
        [DataMember(Name="Valor", EmitDefaultValue=false)]
        public decimal? Valor { get; set; }
        /// <summary>
        /// Ordem em que este produto deve ser mostrado no painel de modificação
        /// </summary>
        /// <value>Ordem em que este produto deve ser mostrado no painel de modificação</value>
        [DataMember(Name="Ordem", EmitDefaultValue=false)]
        public int? Ordem { get; set; }

        public override string ToString()
        {
            return $"{IDProduto} R$ {Valor}";
        }
    }
}
