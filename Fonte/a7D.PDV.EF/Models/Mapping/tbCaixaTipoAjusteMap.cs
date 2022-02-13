using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbCaixaTipoAjusteMap : EntityTypeConfiguration<tbCaixaTipoAjuste>
    {
        public tbCaixaTipoAjusteMap()
        {
            // Primary Key
            this.HasKey(t => t.IDCaixaTipoAjuste);

            // Properties
            this.Property(t => t.IDCaixaTipoAjuste)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("tbCaixaTipoAjuste");
            this.Property(t => t.IDCaixaTipoAjuste).HasColumnName("IDCaixaTipoAjuste");
            this.Property(t => t.Nome).HasColumnName("Nome");
        }
    }
}
