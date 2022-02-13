using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbProdutoTraducaoMap : EntityTypeConfiguration<tbProdutoTraducao>
    {
        public tbProdutoTraducaoMap()
        {
            // Primary Key
            this.HasKey(t => t.IDProdutoTraducao);

            // Properties
            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.Descricao)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("tbProdutoTraducao");
            this.Property(t => t.IDProdutoTraducao).HasColumnName("IDProdutoTraducao");
            this.Property(t => t.IDProduto).HasColumnName("IDProduto");
            this.Property(t => t.IDIdioma).HasColumnName("IDIdioma");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.Descricao).HasColumnName("Descricao");

            // Relationships
            this.HasRequired(t => t.tbIdioma)
                .WithMany(t => t.tbProdutoTraducaos)
                .HasForeignKey(d => d.IDIdioma);
            this.HasRequired(t => t.tbProduto)
                .WithMany(t => t.tbProdutoTraducaos)
                .HasForeignKey(d => d.IDProduto);

        }
    }
}
