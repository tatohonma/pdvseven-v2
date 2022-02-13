using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbPainelModificacaoCategoriaMap : EntityTypeConfiguration<tbPainelModificacaoCategoria>
    {
        public tbPainelModificacaoCategoriaMap()
        {
            // Primary Key
            this.HasKey(t => t.IDPainelModificacaoCategoria);

            // Properties
            // Table & Column Mappings
            this.ToTable("tbPainelModificacaoCategoria");
            this.Property(t => t.IDPainelModificacaoCategoria).HasColumnName("IDPainelModificacaoCategoria");
            this.Property(t => t.IDPainelModificacao).HasColumnName("IDPainelModificacao");
            this.Property(t => t.IDCategoriaProduto).HasColumnName("IDCategoriaProduto");
            this.Property(t => t.Ordem).HasColumnName("Ordem");

            // Relationships
            this.HasRequired(t => t.tbCategoriaProduto)
                .WithMany(t => t.tbPainelModificacaoCategorias)
                .HasForeignKey(d => d.IDCategoriaProduto);
            this.HasRequired(t => t.tbPainelModificacao)
                .WithMany(t => t.tbPainelModificacaoCategorias)
                .HasForeignKey(d => d.IDPainelModificacao);

        }
    }
}
