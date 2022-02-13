using System;
using System.Runtime.Serialization;

namespace a7D.PDV.Integracao.API2.Model
{
    /// <summary>
    /// Categoria
    /// </summary>
    [DataContract]
    public partial class Categoria //: IEquatable<Categoria>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Categoria" /> class.
        /// </summary>
        /// <param name="IDCategoria">IDCategoria.</param>
        /// <param name="Nome">Nome.</param>
        public Categoria(int? idCategoria = default(int?), string nome = default(string), bool disponibilidade = default(bool), DateTime? dtAlteracaoDisponibilidade = default(DateTime?), DateTime? dtUltimaAlteracao = default(DateTime?))
        {
            this.IDCategoria = idCategoria;
            this.Nome = nome;
            this.Disponibilidade = disponibilidade;
            this.DtAlteracaoDisponibilidade = dtAlteracaoDisponibilidade;
            this.DtUltimaAlteracao = dtUltimaAlteracao;
        }

        ///// <summary>
        /// Gets or Sets IDCategoria
        /// </summary>
        [DataMember(Name = "IDCategoria", EmitDefaultValue = false)]
        public int? IDCategoria { get; set; }
        /// <summary>
        /// Gets or Sets Nome
        /// </summary>
        [DataMember(Name = "Nome", EmitDefaultValue = false)]
        public string Nome { get; set; }

        [DataMember(Name = "DtUltimaAlteracao", EmitDefaultValue = false)]
        public DateTime? DtUltimaAlteracao { get; set; }

        [DataMember(Name = "Disponibilidade", EmitDefaultValue = false)]
        public bool Disponibilidade { get; set; }

        [DataMember(Name = "DtAlteracaoDisponibilidade", EmitDefaultValue = false)]
        public DateTime? DtAlteracaoDisponibilidade { get; set; }


    }
}
