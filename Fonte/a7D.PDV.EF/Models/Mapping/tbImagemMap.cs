using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbImagemMap : EntityTypeConfiguration<tbImagem>
    {
        public tbImagemMap()
        {
            // Primary Key
            this.HasKey(t => t.IDImagem);

            // Properties
            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Extensao)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Dados)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("tbImagem");
            this.Property(t => t.IDImagem).HasColumnName("IDImagem");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.Extensao).HasColumnName("Extensao");
            this.Property(t => t.Altura).HasColumnName("Altura");
            this.Property(t => t.Largura).HasColumnName("Largura");
            this.Property(t => t.Tamanho).HasColumnName("Tamanho");
            this.Property(t => t.Dados).HasColumnName("Dados");
        }
    }
}
