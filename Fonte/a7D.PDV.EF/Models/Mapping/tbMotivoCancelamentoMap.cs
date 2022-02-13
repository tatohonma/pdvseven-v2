using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbMotivoCancelamentoMap : EntityTypeConfiguration<tbMotivoCancelamento>
    {
        public tbMotivoCancelamentoMap()
        {
            // Primary Key
            this.HasKey(t => t.IDMotivoCancelamento);

            // Properties
            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("tbMotivoCancelamento");
            this.Property(t => t.IDMotivoCancelamento).HasColumnName("IDMotivoCancelamento");
            this.Property(t => t.Nome).HasColumnName("Nome");
        }
    }
}
