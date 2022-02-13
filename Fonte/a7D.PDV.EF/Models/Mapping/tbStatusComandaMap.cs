using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbStatusComandaMap : EntityTypeConfiguration<tbStatusComanda>
    {
        public tbStatusComandaMap()
        {
            // Primary Key
            this.HasKey(t => t.IDStatusComanda);

            // Properties
            this.Property(t => t.IDStatusComanda)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("tbStatusComanda");
            this.Property(t => t.IDStatusComanda).HasColumnName("IDStatusComanda");
            this.Property(t => t.Nome).HasColumnName("Nome");
        }
    }
}
