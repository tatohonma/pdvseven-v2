using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbConversaoUnidadeMap : EntityTypeConfiguration<tbConversaoUnidade>
    {
        public tbConversaoUnidadeMap()
        {
            // Primary Key
            this.HasKey(t => t.IDConversaoUnidade);

            // Properties
            // Table & Column Mappings
            this.ToTable("tbConversaoUnidade");
            this.Property(t => t.IDConversaoUnidade).HasColumnName("IDConversaoUnidade");
            this.Property(t => t.IDUnidade_de).HasColumnName("IDUnidade_de");
            this.Property(t => t.IDUnidade_para).HasColumnName("IDUnidade_para");
            this.Property(t => t.Divisao).HasColumnName("Divisao").HasPrecision(18, 3);
            this.Property(t => t.Multiplicacao).HasColumnName("Multiplicacao").HasPrecision(18, 3);

            // Relationships
            this.HasRequired(t => t.tbUnidade)
                .WithMany(t => t.tbConversaoUnidades)
                .HasForeignKey(d => d.IDUnidade_de);
            this.HasRequired(t => t.tbUnidade1)
                .WithMany(t => t.tbConversaoUnidades1)
                .HasForeignKey(d => d.IDUnidade_para);

        }
    }
}
