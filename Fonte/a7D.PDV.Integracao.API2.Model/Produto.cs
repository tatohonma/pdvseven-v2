using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace a7D.PDV.Integracao.API2.Model
{
    /// <summary>
    /// Produto
    /// </summary>
    [DataContract]
    public partial class Produto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Produto" /> class.
        /// </summary>
        /// <param name="IDProduto">Chave primária do Produto.</param>
        /// <param name="IDTipoProduto">Tipo do produto 10: Item 20: Modificação 30: Serviço 40: Ingrediente .</param>
        /// <param name="Nome">Nome do produto.</param>
        /// <param name="Descricao">Breve descrição do produto.</param>
        /// <param name="ValorUnitario">ValorUnitario.</param>
        /// <param name="Ativo">Informa se o produto está ou não ativo.</param>
        /// <param name="Excluido">Informa se o produto foi excluído</param>
        /// <param name="DtUltimaAlteracao">Data da última alteração feita no produto.</param>
        /// <param name="Disponibilidade">Indica se o produto está disponível para venda.</param>
        /// <param name="DtAlteracaoDisponibilidade">Data da última alteração feita na disponibilidade desse produto.</param>
        /// <param name="Categorias">Categorias que esse produto pertence.</param>
        /// <param name="PaineisDeModificacao">Painéis de modificação deste produto.</param>
        /// <param name="Receita">Produtos da receita deste produto.</param>
        public Produto(int? IDProduto = default(int?), int? IDTipoProduto = default(int?), string Nome = default(string), string Descricao = default(string), decimal? ValorUnitario = default(decimal?), decimal? ValorUnitario2 = default(decimal?), decimal? ValorUnitario3 = default(decimal?), bool? Ativo = default(bool?), bool? Excluido = default(bool?), bool AssistenteModificacoes = default(bool), DateTime? DtUltimaAlteracao = default(DateTime?), bool? Disponibilidade = default(bool?), DateTime? DtAlteracaoDisponibilidade = default(DateTime?))
        {
            this.IDProduto = IDProduto;
            this.IDTipoProduto = IDTipoProduto;
            this.Nome = Nome;
            this.Descricao = Descricao ?? "";
            this.ValorUnitario = ValorUnitario;
            this.ValorUnitario2 = ValorUnitario2;
            this.ValorUnitario3 = ValorUnitario3;
            this.Ativo = Ativo;
            this.Excluido = Excluido;
            this.AssistenteModificacoes = AssistenteModificacoes;
            this.DtUltimaAlteracao = DtUltimaAlteracao;
            this.Disponibilidade = Disponibilidade;
            this.DtAlteracaoDisponibilidade = DtAlteracaoDisponibilidade;
        }

        [DataMember(Name = "IDProduto", EmitDefaultValue = false)]
        public int? IDProduto { get; set; }
        
        [DataMember(Name = "IDTipoProduto", EmitDefaultValue = false)]
        public int? IDTipoProduto { get; set; }
      
        [DataMember(Name = "Nome", EmitDefaultValue = false)]
        public string Nome { get; set; }
       
        [DataMember(Name = "Descricao", EmitDefaultValue = false)]
        public string Descricao { get; set; }
      
        [DataMember(Name = "ValorUnitario", EmitDefaultValue = false)]
        public decimal? ValorUnitario { get; set; }

        [DataMember(Name = "ValorUnitario2", EmitDefaultValue = false)]
        public decimal? ValorUnitario2 { get; set; }

        [DataMember(Name = "ValorUnitario3", EmitDefaultValue = false)]
        public decimal? ValorUnitario3 { get; set; }

        // Somente em busca por ID
        [DataMember(Name = "Ativo", EmitDefaultValue = false)]
        public bool? Ativo { get; set; }

        // Somente em busca por ID
        [DataMember(Name = "Excluido", EmitDefaultValue = false)]
        public bool? Excluido { get; set; }

        [DataMember(Name = "AssistenteModificacoes", EmitDefaultValue = false)]
        public bool AssistenteModificacoes { get; set; }

        [DataMember(Name = "DtUltimaAlteracao", EmitDefaultValue = false)]
        public DateTime? DtUltimaAlteracao { get; set; }
    
        // A disponibilidade inclu o estado da categoria
        [DataMember(Name = "Disponibilidade", EmitDefaultValue = false)]
        public bool? Disponibilidade { get; set; }
        
        [DataMember(Name = "DtAlteracaoDisponibilidade", EmitDefaultValue = false)]
        public DateTime? DtAlteracaoDisponibilidade { get; set; }

        [DataMember(Name = "urlImagem", EmitDefaultValue = false)]
        public string urlImagem { get; set; }

        [DataMember(Name = "urlImagemThumb", EmitDefaultValue = false)]
        public string urlImagemThumb { get; set; }

        [DataMember(Name = "Categorias", EmitDefaultValue = false)]
        public List<Categoria> Categorias { get; set; }

        [DataMember(Name = "PaineisDeModificacao", EmitDefaultValue = false)]
        public List<PainelDeModificacao> PaineisDeModificacao { get; set; }
        
        [DataMember(Name = "AreasDeImpressao", EmitDefaultValue = false)]
        public List<AreaDeImpressao> AreasDeImpressao { get; set; }

        public override string ToString()
        {
            return $"{IDProduto}: {Nome} R$ {ValorUnitario?.ToString("N2")}";
        }

    }

    [DataContract]
    public partial class ProdutoDisponibilidade
    {
        [DataMember(Name = "IDProduto", EmitDefaultValue = false)]
        public int? IDProduto { get; set; }

        [DataMember(Name = "Disponibilidade", EmitDefaultValue = false)]
        public bool? Disponibilidade { get; set; }

        [DataMember(Name = "DtAlteracaoDisponibilidade", EmitDefaultValue = false)]
        public DateTime? DtAlteracaoDisponibilidade { get; set; }
    }
}
