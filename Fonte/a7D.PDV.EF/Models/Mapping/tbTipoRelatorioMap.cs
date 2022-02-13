using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbTipoRelatorioMap : EntityTypeConfiguration<tbTipoRelatorio>
    {
        public tbTipoRelatorioMap()
        {
            // Primary Key
            this.HasKey(t => t.IDTipoRelatorio);

            // Properties
            this.Property(t => t.IDTipoRelatorio)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Nome)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("tbTipoRelatorio");
            this.Property(t => t.IDTipoRelatorio).HasColumnName("IDTipoRelatorio");
            this.Property(t => t.Nome).HasColumnName("Nome");
        }
    }
}
