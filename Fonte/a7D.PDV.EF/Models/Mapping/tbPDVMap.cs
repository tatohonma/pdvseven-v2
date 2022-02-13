using a7D.PDV.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbPDVMap : EntityTypeConfiguration<PDVInformation>
    {
        public tbPDVMap()
        {
            // Primary Key
            this.HasKey(t => t.IDPDV);

            // Properties
            this.Property(t => t.IDPDV)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Nome)
                .HasMaxLength(50);

            this.Property(t => t.IDPDV).HasColumnName("IDPDV");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.Dados).HasColumnName("Dados");

            this.Ignore(t => t.TipoPDV);
            this.Ignore(t => t.ChaveHardware);
            this.Ignore(t => t.UltimoAcesso);
            this.Ignore(t => t.UltimaAlteracao);
            this.Ignore(t => t.Ativo);
            this.Ignore(t => t.Versao);

            // Table & Column Mappings
            this.ToTable("tbPDV");
            
        }
    }
}
