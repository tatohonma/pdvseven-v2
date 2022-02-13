using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbProdutoImagemMap : EntityTypeConfiguration<tbProdutoImagem>
    {
        public tbProdutoImagemMap()
        {
            // Primary Key
            this.HasKey(t => t.IDProdutoImagem);

            // Properties
            // Table & Column Mappings
            this.ToTable("tbProdutoImagem");
            this.Property(t => t.IDProdutoImagem).HasColumnName("IDProdutoImagem");
            this.Property(t => t.IDProduto).HasColumnName("IDProduto");
            this.Property(t => t.IDImagem).HasColumnName("IDImagem");

            // Relationships
            this.HasRequired(t => t.tbImagem)
                .WithMany(t => t.tbProdutoImagems)
                .HasForeignKey(d => d.IDImagem);
            this.HasRequired(t => t.tbProduto)
                .WithMany(t => t.tbProdutoImagems)
                .HasForeignKey(d => d.IDProduto);

        }
    }
}
