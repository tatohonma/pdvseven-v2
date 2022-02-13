using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbProdutoPainelModificacaoMap : EntityTypeConfiguration<tbProdutoPainelModificacao>
    {
        public tbProdutoPainelModificacaoMap()
        {
            // Primary Key
            this.HasKey(t => t.IDProdutoPainelModificacao);

            // Properties
            // Table & Column Mappings
            this.ToTable("tbProdutoPainelModificacao");
            this.Property(t => t.IDProdutoPainelModificacao).HasColumnName("IDProdutoPainelModificacao");
            this.Property(t => t.IDProduto).HasColumnName("IDProduto");
            this.Property(t => t.IDPainelModificacao).HasColumnName("IDPainelModificacao");
            this.Property(t => t.Ordem).HasColumnName("Ordem");

            // Relationships
            this.HasRequired(t => t.tbPainelModificacao)
                .WithMany(t => t.tbProdutoPainelModificacaos)
                .HasForeignKey(d => d.IDPainelModificacao);
            this.HasRequired(t => t.tbProduto)
                .WithMany(t => t.tbProdutoPainelModificacaos)
                .HasForeignKey(d => d.IDProduto);

        }
    }
}
