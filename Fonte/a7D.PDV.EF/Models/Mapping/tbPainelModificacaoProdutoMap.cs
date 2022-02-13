using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbPainelModificacaoProdutoMap : EntityTypeConfiguration<tbPainelModificacaoProduto>
    {
        public tbPainelModificacaoProdutoMap()
        {
            // Primary Key
            this.HasKey(t => t.IDPainelModificacaoProduto);

            // Properties
            // Table & Column Mappings
            this.ToTable("tbPainelModificacaoProduto");
            this.Property(t => t.IDPainelModificacaoProduto).HasColumnName("IDPainelModificacaoProduto");
            this.Property(t => t.IDPainelModificacao).HasColumnName("IDPainelModificacao");
            this.Property(t => t.IDProduto).HasColumnName("IDProduto");
            this.Property(t => t.IgnorarValorItem).HasColumnName("IgnorarValorItem");
            this.Property(t => t.Ordem).HasColumnName("Ordem");

            // Relationships
            this.HasRequired(t => t.tbPainelModificacao)
                .WithMany(t => t.tbPainelModificacaoProdutoes)
                .HasForeignKey(d => d.IDPainelModificacao);
            this.HasRequired(t => t.tbProduto)
                .WithMany(t => t.tbPainelModificacaoProdutoes)
                .HasForeignKey(d => d.IDProduto);

        }
    }
}
