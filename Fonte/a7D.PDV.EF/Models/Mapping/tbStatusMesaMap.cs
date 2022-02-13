using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbStatusMesaMap : EntityTypeConfiguration<tbStatusMesa>
    {
        public tbStatusMesaMap()
        {
            // Primary Key
            this.HasKey(t => t.IDStatusMesa);

            // Properties
            this.Property(t => t.IDStatusMesa)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("tbStatusMesa");
            this.Property(t => t.IDStatusMesa).HasColumnName("IDStatusMesa");
            this.Property(t => t.Nome).HasColumnName("Nome");
        }
    }
}
