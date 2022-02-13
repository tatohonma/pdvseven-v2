using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbEstadoMap : EntityTypeConfiguration<tbEstado>
    {
        public tbEstadoMap()
        {
            // Primary Key
            this.HasKey(t => t.IDEstado);

            // Properties
            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Sigla)
                .IsRequired()
                .HasMaxLength(2);

            // Table & Column Mappings
            this.ToTable("tbEstado");
            this.Property(t => t.IDEstado).HasColumnName("IDEstado");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.Sigla).HasColumnName("Sigla");
        }
    }
}
