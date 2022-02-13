using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbProdutoCategoriaProdutoMap : EntityTypeConfiguration<tbProdutoCategoriaProduto>
    {
        public tbProdutoCategoriaProdutoMap()
        {
            // Primary Key
            this.HasKey(t => t.IDProdutoCategoriaProduto);

            // Properties
            // Table & Column Mappings
            this.ToTable("tbProdutoCategoriaProduto");
            this.Property(t => t.IDProdutoCategoriaProduto).HasColumnName("IDProdutoCategoriaProduto");
            this.Property(t => t.IDProduto).HasColumnName("IDProduto");
            this.Property(t => t.IDCategoriaProduto).HasColumnName("IDCategoriaProduto");

            // Relationships
            this.HasRequired(t => t.tbCategoriaProduto)
                .WithMany(t => t.tbProdutoCategoriaProdutoes)
                .HasForeignKey(d => d.IDCategoriaProduto);
            this.HasRequired(t => t.tbProduto)
                .WithMany(t => t.tbProdutoCategoriaProdutoes)
                .HasForeignKey(d => d.IDProduto);

        }
    }
}
