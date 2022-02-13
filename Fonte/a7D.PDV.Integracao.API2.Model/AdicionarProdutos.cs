using System.Collections.Generic;
using System.Runtime.Serialization;

namespace a7D.PDV.Integracao.API2.Model
{
    /// <summary>
    /// AdicionarProdutos
    /// </summary>
    [DataContract]
    public partial class AdicionarProdutos 
    {

        public AdicionarProdutos()
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="AdicionarProdutos" /> class.
        /// </summary>
        /// <param name="GUIDISolicitacao">GUID desta solicitação de adição de produtos. GUID para controle de duplicidade de chamadas.</param>
        /// <param name="IDTipoPedido">Tipo do pedido onde os itens estão sendo inseridos 10: Mesa 20: Comanda .</param>
        /// <param name="Numero">Numero da mesa ou comanda onde serão inseridos os produtos.</param>
        /// <param name="IDUsuario">ID do usuário que está solicitando esta adição de produtos.</param>
        /// <param name="IDPDV">ID do PDV que está solicitando está adição de produtos.</param>
        /// <param name="Itens">Itens.</param>
        public AdicionarProdutos(string GUIDISolicitacao, int IDTipoPedido, object Numero, int? IDUsuario = default(int?), int? IDPDV = default(int?), List<Item> Itens = default(List<Item>))
        {
            this.GUIDSolicitacao = GUIDISolicitacao;
            this.IDTipoPedido = IDTipoPedido;
            this.Numero = Numero;
            this.IDUsuario = IDUsuario;
            this.IDPDV = IDPDV;
            this.Itens = Itens;
        }

        /// <summary>
        /// GUID desta solicitação de adição de produtos. GUID para controle de duplicidade de chamadas
        /// </summary>
        /// <value>GUID desta solicitação de adição de produtos. GUID para controle de duplicidade de chamadas</value>
        [DataMember(Name = "GUIDSolicitacao", EmitDefaultValue = false)]
        public string GUIDSolicitacao { get; set; }
        /// <summary>
        /// Tipo do pedido onde os itens estão sendo inseridos 10: Mesa 20: Comanda 
        /// </summary>
        /// <value>Tipo do pedido onde os itens estão sendo inseridos 10: Mesa 20: Comanda </value>
        [DataMember(Name = "IDTipoPedido", EmitDefaultValue = false)]
        public int IDTipoPedido { get; set; }
        /// <summary>
        /// Numero da mesa ou comanda onde serão inseridos os produtos
        /// </summary>
        /// <value>Numero da mesa, comanda, ou IDdoCliente onde serão inseridos os produtos</value>
        [DataMember(Name = "Numero", EmitDefaultValue = false)]
        public object Numero { get; set; }
        /// <summary>
        /// ID do usuário que está solicitando esta adição de produtos
        /// </summary>
        /// <value>ID do usuário que está solicitando esta adição de produtos</value>
        [DataMember(Name = "IDUsuario", EmitDefaultValue = false)]
        public int? IDUsuario { get; set; }
        /// <summary>
        /// ID do PDV que está solicitando está adição de produtos
        /// </summary>
        /// <value>ID do PDV que está solicitando está adição de produtos</value>
        [DataMember(Name = "IDPDV", EmitDefaultValue = false)]
        public int? IDPDV { get; set; }
        /// <summary>
        /// Gets or Sets Itens
        /// </summary>
        [DataMember(Name = "Itens", EmitDefaultValue = false)]
        public List<Item> Itens { get; set; }

        /// <summary>
        /// Indica se é para produzir (Ordem de Produção)
        /// </summary>
        [DataMember(Name = "GerarOrdemProducao", EmitDefaultValue = false)]
        public bool? GerarOrdemProducao { get; set; }

        /// <summary>
        /// Para Balança ou Choppera não pode vaidar os limites da comanda
        /// </summary>
        [DataMember(Name = "ValidarLimite", EmitDefaultValue = false)]
        public bool? ValidarLimite { get; set; }
    }
}
