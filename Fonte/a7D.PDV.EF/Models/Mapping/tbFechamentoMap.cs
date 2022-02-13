using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbFechamentoMap : EntityTypeConfiguration<tbFechamento>
    {
        public tbFechamentoMap()
        {
            // Primary Key
            this.HasKey(t => t.IDFechamento);

            // Properties
            // Table & Column Mappings
            this.ToTable("tbFechamento");
            this.Property(t => t.IDFechamento).HasColumnName("IDFechamento");
            this.Property(t => t.IDPDV).HasColumnName("IDPDV");
            this.Property(t => t.IDUsuario).HasColumnName("IDUsuario");
            this.Property(t => t.DtFechamento).HasColumnName("DtFechamento");

            // Relationships
            this.HasRequired(t => t.tbPDV)
                .WithMany(t => t.tbFechamentoes)
                .HasForeignKey(d => d.IDPDV);
            this.HasRequired(t => t.tbUsuario)
                .WithMany(t => t.tbFechamentoes)
                .HasForeignKey(d => d.IDUsuario);

        }
    }
}
