using System.Collections.Generic;
using System.Runtime.Serialization;

namespace a7D.PDV.Integracao.API2.Model
{
    /// <summary>
    /// PainelDeModificacao
    /// </summary>
    [DataContract]
    public partial class PainelDeModificacao 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PainelDeModificacao" /> class.
        /// </summary>
        /// <param name="IDPainelModificacao">Chave primária do Painel de Modificacao.</param>
        /// <param name="Nome">Nome do painel de modificação.</param>
        /// <param name="Min">Quantidade mínima de modificaçãoes a serem escolhidas.</param>
        /// <param name="Max">Quantidade máxima de modificações a serem escolhidas.</param>
        /// <param name="Titulo">Nome alternativo a ser exibido nas aplicações.</param>
        /// <param name="Produtos">Produtos.</param>
        public PainelDeModificacao(int? IDPainelModificacao = default(int?), string Nome = default(string), int? Min = default(int?), int? Max = default(int?), string Titulo = default(string), List<ProdutoPainelModificacao> Produtos = default(List<ProdutoPainelModificacao>))
        {
            this.IDPainelModificacao = IDPainelModificacao;
            this.Nome = Nome;
            this.Min = Min;
            this.Max = Max;
            this.Titulo = Titulo;
            this.Produtos = Produtos;
        }
        
        /// <summary>
        /// Chave primária do Painel de Modificacao
        /// </summary>
        /// <value>Chave primária do Painel de Modificacao</value>
        [DataMember(Name="IDPainelModificacao", EmitDefaultValue=false)]
        public int? IDPainelModificacao { get; set; }
        /// <summary>
        /// Nome do painel de modificação
        /// </summary>
        /// <value>Nome do painel de modificação</value>
        [DataMember(Name="Nome", EmitDefaultValue=false)]
        public string Nome { get; set; }
        /// <summary>
        /// Quantidade mínima de modificaçãoes a serem escolhidas
        /// </summary>
        /// <value>Quantidade mínima de modificaçãoes a serem escolhidas</value>
        [DataMember(Name="Min", EmitDefaultValue=false)]
        public int? Min { get; set; }
        /// <summary>
        /// Quantidade máxima de modificações a serem escolhidas
        /// </summary>
        /// <value>Quantidade máxima de modificações a serem escolhidas</value>
        [DataMember(Name="Max", EmitDefaultValue=false)]
        public int? Max { get; set; }
        /// <summary>
        /// Nome alternativo a ser exibido nas aplicações
        /// </summary>
        /// <value>Nome alternativo a ser exibido nas aplicações</value>
        [DataMember(Name="Titulo", EmitDefaultValue=false)]
        public string Titulo { get; set; }
        /// <summary>
        /// Gets or Sets Produtos
        /// </summary>
        [DataMember(Name="Produtos", EmitDefaultValue=false)]
        public List<ProdutoPainelModificacao> Produtos { get; set; }

        public override string ToString()
        {
            return $"{Titulo} {Min}/{Max}";
        }
    }
}
