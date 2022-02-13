using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbClassificacaoFiscalMap : EntityTypeConfiguration<tbClassificacaoFiscal>
    {
        public tbClassificacaoFiscalMap()
        {
            // Primary Key
            this.HasKey(t => t.IDClassificacaoFiscal);

            // Properties
            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Descricao)
                .HasMaxLength(500);

            this.Property(t => t.NCM)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("tbClassificacaoFiscal");
            this.Property(t => t.IDClassificacaoFiscal).HasColumnName("IDClassificacaoFiscal");
            this.Property(t => t.IDTipoTributacao).HasColumnName("IDTipoTributacao");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.Descricao).HasColumnName("Descricao");
            this.Property(t => t.NCM).HasColumnName("NCM");
            this.Property(t => t.CEST).HasColumnName("CEST");
            this.Property(t => t.IOF).HasColumnName("IOF");
            this.Property(t => t.IPI).HasColumnName("IPI");
            this.Property(t => t.PISPASEP).HasColumnName("PISPASEP");
            this.Property(t => t.CIDE).HasColumnName("CIDE");
            this.Property(t => t.COFINS).HasColumnName("COFINS");
            this.Property(t => t.ICMS).HasColumnName("ICMS");
            this.Property(t => t.ISS).HasColumnName("ISS");

            // Relationships
            this.HasRequired(t => t.tbTipoTributacao)
                .WithMany(t => t.tbClassificacaoFiscals)
                .HasForeignKey(d => d.IDTipoTributacao);

        }
    }
}
