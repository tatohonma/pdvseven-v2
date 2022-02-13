using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbRelatorioMap : EntityTypeConfiguration<tbRelatorio>
    {
        public tbRelatorioMap()
        {
            // Primary Key
            this.HasKey(t => t.IDRelatorio);

            // Properties
            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.Descricao)
                .HasMaxLength(500);

            this.Property(t => t.QuerySQL)
                .IsRequired();

            this.Property(t => t.Totalizador)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("tbRelatorio");
            this.Property(t => t.IDRelatorio).HasColumnName("IDRelatorio");
            this.Property(t => t.IDTipoRelatorio).HasColumnName("IDTipoRelatorio");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.Descricao).HasColumnName("Descricao");
            this.Property(t => t.QuerySQL).HasColumnName("QuerySQL");
            this.Property(t => t.Ordem).HasColumnName("Ordem");
            this.Property(t => t.Totalizador).HasColumnName("Totalizador");

            // Relationships
            this.HasRequired(t => t.tbTipoRelatorio)
                .WithMany(t => t.tbRelatorios)
                .HasForeignKey(d => d.IDTipoRelatorio);

        }
    }
}
