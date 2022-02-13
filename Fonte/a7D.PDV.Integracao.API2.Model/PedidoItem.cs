using System.Collections.Generic;
using System.Runtime.Serialization;

namespace a7D.PDV.Integracao.API2.Model
{
    /// <summary>
    /// PedidoItem
    /// </summary>
    [DataContract]
    public partial class PedidoItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PedidoItem" /> class.
        /// </summary>
        /// <param name="IDPedido">Chave primária deste item deste pedido.</param>
        /// <param name="IDPdv">ID do produto deste item.</param>
        /// <param name="ChaveAcesso">Quantidade deste item.</param>
        /// <param name="Itens">Modificações (IDProduto) selecionadas deste item.</param>


        public PedidoItem(int? IDPedido = default(int?),
            int? IDPdv = default(int?),
            string ChaveAcesso = default(string),
            string Notas = default(string),
            List<Item> Itens = default(List<Item>))
        {
            this.IDPedido = IDPedido;
            this.IDPdv = IDPdv;
            this.ChaveAcesso = ChaveAcesso;
            this.Itens = Itens;
            this.Notas = Notas;
        }


        [DataMember(Name = "IDPedido", EmitDefaultValue = false)]
        public int? IDPedido { get; set; }

        [DataMember(Name = "IDPdv", EmitDefaultValue = false)]
        public int? IDPdv { get; set; }

        [DataMember(Name = "ChaveAcesso", EmitDefaultValue = false)]
        public string ChaveAcesso { get; set; }

        [DataMember(Name = "Notas", EmitDefaultValue = false)]
        public string Notas { get; set; }

        [DataMember(Name = "Itens", EmitDefaultValue = false)]
        public List<Item> Itens { get; set; }
    }
}
