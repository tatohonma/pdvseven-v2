using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbProdutoReceitaMap : EntityTypeConfiguration<tbProdutoReceita>
    {
        public tbProdutoReceitaMap()
        {
            // Primary Key
            this.HasKey(t => t.IDProdutoReceita);

            // Properties
            // Table & Column Mappings
            this.ToTable("tbProdutoReceita");
            this.Property(t => t.IDProdutoReceita).HasColumnName("IDProdutoReceita");
            this.Property(t => t.IDProduto).HasColumnName("IDProduto");
            this.Property(t => t.IDProduto_ingrediente).HasColumnName("IDProduto_ingrediente");
            this.Property(t => t.IDUnidade).HasColumnName("IDUnidade");
            this.Property(t => t.Quantidade).HasColumnName("Quantidade").HasPrecision(18, 3);

            // Relationships
            this.HasRequired(t => t.tbProduto)
                .WithMany(t => t.tbProdutoReceitas)
                .HasForeignKey(d => d.IDProduto);
            this.HasRequired(t => t.tbProduto1)
                .WithMany(t => t.tbProdutoReceitas1)
                .HasForeignKey(d => d.IDProduto_ingrediente);
            this.HasRequired(t => t.tbUnidade)
                .WithMany(t => t.tbProdutoReceitas)
                .HasForeignKey(d => d.IDUnidade);

        }
    }
}
