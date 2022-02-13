using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    internal class tbBandeiraMap : EntityTypeConfiguration<tbBandeira>
    {
        public tbBandeiraMap()
        {
            this.HasKey(p => p.IDBandeira);

            this.Property(p => p.IDBandeira)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Nome)
                .HasMaxLength(50)
                .IsRequired();

            this.ToTable("tbBandeira");
        }
    }
}