using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbHorarioDeliveryMap : EntityTypeConfiguration<tbHorarioDelivery>
    {
        public tbHorarioDeliveryMap()
        {
            // Primary Key
            this.HasKey(t => t.DiaSemana);

            // Properties
            this.Property(t => t.DiaSemana)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Turno1Inicio).HasColumnName("Turno1Inicio");
            this.Property(t => t.Turno1Fim).HasColumnName("Turno1Fim");

            this.Property(t => t.Turno2Inicio).HasColumnName("Turno2Inicio");
            this.Property(t => t.Turno2Fim).HasColumnName("Turno2Fim");

            this.Property(t => t.Turno3Inicio).HasColumnName("Turno3Inicio");
            this.Property(t => t.Turno3Fim).HasColumnName("Turno3Fim");

            // Table & Column Mappings
            this.ToTable("tbHorarioDelivery");
        }
    }
}
