using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbProcessamentoSATMap : EntityTypeConfiguration<tbProcessamentoSAT>
    {
        public tbProcessamentoSATMap()
        {
            // Primary Key
            this.HasKey(t => t.IDProcessamentoSAT);

            // Properties
            this.Property(t => t.XMLEnvio)
                .IsRequired();

            this.Property(t => t.GUID)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("tbProcessamentoSAT");
            this.Property(t => t.IDProcessamentoSAT).HasColumnName("IDProcessamentoSAT");
            this.Property(t => t.IDStatusProcessamentoSAT).HasColumnName("IDStatusProcessamentoSAT");
            this.Property(t => t.IDTipoSolicitacaoSAT).HasColumnName("IDTipoSolicitacaoSAT");
            this.Property(t => t.IDRetornoSAT).HasColumnName("IDRetornoSAT");
            this.Property(t => t.XMLEnvio).HasColumnName("XMLEnvio");
            this.Property(t => t.GUID).HasColumnName("GUID");
            this.Property(t => t.NumeroSessao).HasColumnName("NumeroSessao");
            this.Property(t => t.DataSolicitacao).HasColumnName("DataSolicitacao");

            // Relationships
            this.HasOptional(t => t.tbRetornoSAT)
                .WithMany(t => t.tbProcessamentoSATs)
                .HasForeignKey(d => d.IDRetornoSAT);
            this.HasOptional(t => t.tbStatusProcessamentoSAT)
                .WithMany(t => t.tbProcessamentoSATs)
                .HasForeignKey(d => d.IDStatusProcessamentoSAT);
            this.HasOptional(t => t.tbTipoSolicitacaoSAT)
                .WithMany(t => t.tbProcessamentoSATs)
                .HasForeignKey(d => d.IDTipoSolicitacaoSAT);

        }
    }
}
