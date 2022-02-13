using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbIdiomaMap : EntityTypeConfiguration<tbIdioma>
    {
        public tbIdiomaMap()
        {
            // Primary Key
            this.HasKey(t => t.IDIdioma);

            // Properties
            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Codigo)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("tbIdioma");
            this.Property(t => t.IDIdioma).HasColumnName("IDIdioma");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.Codigo).HasColumnName("Codigo");
        }
    }
}
