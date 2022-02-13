using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbRetornoSATMap : EntityTypeConfiguration<tbRetornoSAT>
    {
        public tbRetornoSATMap()
        {
            // Primary Key
            this.HasKey(t => t.IDRetornoSAT);

            // Properties
            // Table & Column Mappings
            this.ToTable("tbRetornoSAT");
            this.Property(t => t.IDRetornoSAT).HasColumnName("IDRetornoSAT");
            this.Property(t => t.IDTipoSolicitacaoSAT).HasColumnName("IDTipoSolicitacaoSAT");
            this.Property(t => t.numeroSessao).HasColumnName("numeroSessao");
            this.Property(t => t.EEEEE).HasColumnName("EEEEE");
            this.Property(t => t.CCCC).HasColumnName("CCCC");
            this.Property(t => t.mensagem).HasColumnName("mensagem");
            this.Property(t => t.cod).HasColumnName("cod");
            this.Property(t => t.mensagemSEFAZ).HasColumnName("mensagemSEFAZ");
            this.Property(t => t.arquivoCFeSAT).HasColumnName("arquivoCFeSAT");
            this.Property(t => t.timeStamp).HasColumnName("timeStamp");
            this.Property(t => t.chaveConsulta).HasColumnName("chaveConsulta");
            this.Property(t => t.valorTotalCFe).HasColumnName("valorTotalCFe");
            this.Property(t => t.CPFCNPJValue).HasColumnName("CPFCNPJValue");
            this.Property(t => t.assinaturaQRCODE).HasColumnName("assinaturaQRCODE");
            this.Property(t => t.IDRetornoSAT_cancelamento).HasColumnName("IDRetornoSAT_cancelamento");

            // Relationships
            this.HasOptional(t => t.tbRetornoSAT2)
                .WithMany(t => t.tbRetornoSAT1)
                .HasForeignKey(d => d.IDRetornoSAT_cancelamento);
            this.HasRequired(t => t.tbTipoSolicitacaoSAT)
                .WithMany(t => t.tbRetornoSATs)
                .HasForeignKey(d => d.IDTipoSolicitacaoSAT);

        }
    }
}
