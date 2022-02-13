using a7D.PDV.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbTipoPDVMap : EntityTypeConfiguration<TipoPDVInformation>
    {
        public tbTipoPDVMap()
        {
            // Primary Key
            this.HasKey(t => t.IDTipoPDV);

            // Properties
            this.Property(t => t.IDTipoPDV)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.IDTipoPDV).HasColumnName("IDTipoPDV");
            this.Property(t => t.Nome).HasColumnName("Nome");

            // Table & Column Mappings
            this.ToTable("tbTipoPDV");
        }
    }
}
