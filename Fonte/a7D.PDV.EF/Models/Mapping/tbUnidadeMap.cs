using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbUnidadeMap : EntityTypeConfiguration<tbUnidade>
    {
        public tbUnidadeMap()
        {
            // Primary Key
            this.HasKey(t => t.IDUnidade);

            // Properties
            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Simbolo)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("tbUnidade");
            this.Property(t => t.IDUnidade).HasColumnName("IDUnidade");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.Simbolo).HasColumnName("Simbolo");
            this.Property(t => t.Excluido).HasColumnName("Excluido");
        }
    }
}
