using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbImagemTemaMap : EntityTypeConfiguration<tbImagemTema>
    {
        public tbImagemTemaMap()
        {
            // Primary Key
            this.HasKey(t => t.IDImagemTema);

            // Properties
            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Descricao)
                .HasMaxLength(300);

            // Table & Column Mappings
            this.ToTable("tbImagemTema");
            this.Property(t => t.IDImagemTema).HasColumnName("IDImagemTema");
            this.Property(t => t.IDTemaCardapio).HasColumnName("IDTemaCardapio");
            this.Property(t => t.IDImagem).HasColumnName("IDImagem");
            this.Property(t => t.IDIdioma).HasColumnName("IDIdioma");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.Descricao).HasColumnName("Descricao");

            // Relationships
            this.HasRequired(t => t.tbIdioma)
                .WithMany(t => t.tbImagemTemas)
                .HasForeignKey(d => d.IDIdioma);
            this.HasRequired(t => t.tbImagem)
                .WithMany(t => t.tbImagemTemas)
                .HasForeignKey(d => d.IDImagem);
            this.HasRequired(t => t.tbTemaCardapio)
                .WithMany(t => t.tbImagemTemas)
                .HasForeignKey(d => d.IDTemaCardapio);

        }
    }
}
