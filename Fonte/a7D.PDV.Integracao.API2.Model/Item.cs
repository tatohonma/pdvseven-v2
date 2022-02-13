using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace a7D.PDV.Integracao.API2.Model
{
    /// <summary>
    /// Item
    /// </summary>
    [DataContract]
    public partial class Item
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Item" /> class.
        /// </summary>
        /// <param name="ID">Chave primária deste item deste pedido.</param>
        /// <param name="IDProduto">ID do produto deste item.</param>
        /// <param name="Qtd">Quantidade deste item.</param>
        /// <param name="Modificacoes">Modificações (IDProduto) selecionadas deste item.</param>
        /// <param name="Notas">Notas deste item.</param>
        /// <param name="Preco">Preço do item. Se o preço do produto for 0, é possível especificar o novo preço do produto por este campo..</param>
        public Item(int? ID = default(int?), int? IDProduto = default(int?), string Descricao = default(string), decimal? Qtd = default(decimal?), List<Item> Modificacoes = default, string Notas = default(string), decimal? Preco = default(decimal?))
        {
            this.ID = ID;
            this.IDProduto = IDProduto;
            this.Descricao = Descricao;
            this.Qtd = Qtd;
            this.Modificacoes = Modificacoes;
            this.Notas = Notas;
            this.Preco = Preco;
        }

        public Item()
        {

        }

        /// <summary>
        /// Chave primária deste item deste pedido
        /// </summary>
        /// <value>Chave primária deste item deste pedido</value>
        [DataMember(Name = "ID", EmitDefaultValue = false)]
        public int? ID { get; set; }
        /// <summary>
        /// ID do produto deste item
        /// </summary>
        /// <value>ID do produto deste item</value>
        [DataMember(Name = "IDProduto", EmitDefaultValue = false)]
        public int? IDProduto { get; set; }
        /// <summary>
        /// Quantidade deste item
        /// </summary>
        /// <value>Quantidade deste item</value>
        [DataMember(Name = "Qtd", EmitDefaultValue = false)]
        public decimal? Qtd { get; set; }
        /// <summary>
        /// Modificações (IDProduto) selecionadas deste item
        /// </summary>
        /// <value>Modificações (IDProduto) selecionadas deste item</value>
        [DataMember(Name = "Modificacoes", EmitDefaultValue = false)]
        public List<Item> Modificacoes { get; set; }
        /// <summary>
        /// Notas deste item
        /// </summary>
        /// <value>Notas deste item</value>
        [DataMember(Name = "Notas", EmitDefaultValue = false)]
        public string Notas { get; set; }
        /// <summary>
        /// Preço do item. Se o preço do produto for 0, é possível especificar o novo preço do produto por este campo.
        /// </summary>
        /// <value>Preço do item. Se o preço do produto for 0, é possível especificar o novo preço do produto por este campo.</value>
        [DataMember(Name = "Preco", EmitDefaultValue = false)]
        public decimal? Preco { get; set; }
        /// <summary>
        /// Descrição do item
        /// </summary>
        /// <value>Descrição do item</value>
        [DataMember(Name = "Descricao", EmitDefaultValue = false)]
        public string Descricao { get; set; }

        [IgnoreDataMember]
        public decimal ValorTotal => Math.Truncate((Qtd ?? 0) * (Preco ?? 0) * 100m) / 100m;

        public override string ToString()
        {
            return $"{ID}: {Qtd} x {Descricao ?? IDProduto?.ToString()} R$ {Preco?.ToString("N2")} = R$ {ValorTotal.ToString("N2")}";
        }
    }
}
