using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbMovimentacaoProdutoMap : EntityTypeConfiguration<tbMovimentacaoProduto>
    {
        public tbMovimentacaoProdutoMap()
        {
            // Primary Key
            this.HasKey(t => t.IDMovimentacaoProdutos);

            // Properties
            // Table & Column Mappings
            this.ToTable("tbMovimentacaoProdutos");
            this.Property(t => t.IDMovimentacaoProdutos).HasColumnName("IDMovimentacaoProdutos");
            this.Property(t => t.IDMovimentacao).HasColumnName("IDMovimentacao");
            this.Property(t => t.IDProduto).HasColumnName("IDProduto");
            this.Property(t => t.IDUnidade).HasColumnName("IDUnidade");
            this.Property(t => t.Quantidade).HasColumnName("Quantidade").HasPrecision(18, 3);

            // Relationships
            this.HasRequired(t => t.tbMovimentacao)
                .WithMany(t => t.tbMovimentacaoProdutos)
                .HasForeignKey(d => d.IDMovimentacao);
            this.HasRequired(t => t.tbProduto)
                .WithMany(t => t.tbMovimentacaoProdutos)
                .HasForeignKey(d => d.IDProduto);
            this.HasRequired(t => t.tbUnidade)
                .WithMany(t => t.tbMovimentacaoProdutos)
                .HasForeignKey(d => d.IDUnidade);

        }
    }
}
