using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbTemaCardapioMap : EntityTypeConfiguration<tbTemaCardapio>
    {
        public tbTemaCardapioMap()
        {
            // Primary Key
            this.HasKey(t => t.IDTemaCardapio);

            // Properties
            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.XML)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("tbTemaCardapio");
            this.Property(t => t.IDTemaCardapio).HasColumnName("IDTemaCardapio");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.Descricao).HasColumnName("Descricao");
            this.Property(t => t.XML).HasColumnName("XML");
        }
    }
}
