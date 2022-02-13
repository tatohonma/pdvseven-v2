using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbInventarioProdutoMap : EntityTypeConfiguration<tbInventarioProduto>
    {
        public tbInventarioProdutoMap()
        {
            // Primary Key
            this.HasKey(t => t.IDInventarioProdutos);

            // Properties
            // Table & Column Mappings
            this.ToTable("tbInventarioProdutos");
            this.Property(t => t.IDInventarioProdutos).HasColumnName("IDInventarioProdutos");
            this.Property(t => t.IDInventario).HasColumnName("IDInventario");
            this.Property(t => t.IDProduto).HasColumnName("IDProduto");
            this.Property(t => t.IDUnidade).HasColumnName("IDUnidade");
            this.Property(t => t.Quantidade).HasColumnName("Quantidade").HasPrecision(18, 3);
            this.Property(t => t.QuantidadeAnterior).HasColumnName("QuantidadeAnterior").HasPrecision(18, 3);

            // Relationships
            this.HasRequired(t => t.tbInventario)
                .WithMany(t => t.tbInventarioProdutos)
                .HasForeignKey(d => d.IDInventario);
            this.HasRequired(t => t.tbProduto)
                .WithMany(t => t.tbInventarioProdutos)
                .HasForeignKey(d => d.IDProduto);
            this.HasRequired(t => t.tbUnidade)
                .WithMany(t => t.tbInventarioProdutos)
                .HasForeignKey(d => d.IDUnidade);

        }
    }
}
