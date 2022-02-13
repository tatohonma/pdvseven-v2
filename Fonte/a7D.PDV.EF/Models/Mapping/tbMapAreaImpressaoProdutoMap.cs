using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbMapAreaImpressaoProdutoMap : EntityTypeConfiguration<tbMapAreaImpressaoProduto>
    {
        public tbMapAreaImpressaoProdutoMap()
        {
            // Primary Key
            this.HasKey(t => t.IDMapAreaImpressaoProduto);

            // Properties
            // Table & Column Mappings
            this.ToTable("tbMapAreaImpressaoProduto");
            this.Property(t => t.IDMapAreaImpressaoProduto).HasColumnName("IDMapAreaImpressaoProduto");
            this.Property(t => t.IDAreaImpressao).HasColumnName("IDAreaImpressao");
            this.Property(t => t.IDProduto).HasColumnName("IDProduto");

            // Relationships
            this.HasRequired(t => t.tbAreaImpressao)
                .WithMany(t => t.tbMapAreaImpressaoProdutoes)
                .HasForeignKey(d => d.IDAreaImpressao);
            this.HasRequired(t => t.tbProduto)
                .WithMany(t => t.tbMapAreaImpressaoProdutoes)
                .HasForeignKey(d => d.IDProduto);

        }
    }
}
