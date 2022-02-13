using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbPainelModificacaoMap : EntityTypeConfiguration<tbPainelModificacao>
    {
        public tbPainelModificacaoMap()
        {
            // Primary Key
            this.HasKey(t => t.IDPainelModificacao);

            // Properties
            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.Titulo)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("tbPainelModificacao");
            this.Property(t => t.IDPainelModificacao).HasColumnName("IDPainelModificacao");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.Titulo).HasColumnName("Titulo");
            this.Property(t => t.Minimo).HasColumnName("Minimo");
            this.Property(t => t.Maximo).HasColumnName("Maximo");
            this.Property(t => t.DtUltimaAlteracao).HasColumnName("DtUltimaAlteracao");
            this.Property(t => t.Excluido).HasColumnName("Excluido");
            this.Property(t => t.IDPainelModificacaoOperacao).HasColumnName("IDPainelModificacaoOperacao");
            this.Property(t => t.IDTipoItem).HasColumnName("IDTipoItem");
            this.Property(t => t.IDValorUtilizado).HasColumnName("IDValorUtilizado");
            this.Property(t => t.IgnorarValorItem).HasColumnName("IgnorarValorItem");

            // Relationships
            this.HasOptional(t => t.tbPainelModificacaoOperacao)
                .WithMany(t => t.tbPainelModificacaos)
                .HasForeignKey(d => d.IDPainelModificacaoOperacao);

        }
    }
}
