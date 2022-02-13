using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbStatusProcessamentoSATMap : EntityTypeConfiguration<tbStatusProcessamentoSAT>
    {
        public tbStatusProcessamentoSATMap()
        {
            // Primary Key
            this.HasKey(t => t.IDStatusProcessamentoSAT);

            // Properties
            this.Property(t => t.IDStatusProcessamentoSAT)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("tbStatusProcessamentoSAT");
            this.Property(t => t.IDStatusProcessamentoSAT).HasColumnName("IDStatusProcessamentoSAT");
            this.Property(t => t.Nome).HasColumnName("Nome");
        }
    }
}
