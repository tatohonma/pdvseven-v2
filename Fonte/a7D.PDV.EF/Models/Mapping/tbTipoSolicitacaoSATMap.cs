using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbTipoSolicitacaoSATMap : EntityTypeConfiguration<tbTipoSolicitacaoSAT>
    {
        public tbTipoSolicitacaoSATMap()
        {
            // Primary Key
            this.HasKey(t => t.IDTipoSolicitacaoSAT);

            // Properties
            this.Property(t => t.IDTipoSolicitacaoSAT)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("tbTipoSolicitacaoSAT");
            this.Property(t => t.IDTipoSolicitacaoSAT).HasColumnName("IDTipoSolicitacaoSAT");
            this.Property(t => t.Nome).HasColumnName("Nome");
        }
    }
}
