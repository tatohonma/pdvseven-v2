using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbOrdemImpressaoMap : EntityTypeConfiguration<tbOrdemImpressao>
    {
        public tbOrdemImpressaoMap()
        {
            // Primary Key
            this.HasKey(t => t.IDOrdemImpressao);

            // Properties
            this.Property(t => t.ConteudoImpressao)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("tbOrdemImpressao");
            this.Property(t => t.IDOrdemImpressao).HasColumnName("IDOrdemImpressao");
            this.Property(t => t.IDAreaImpressao).HasColumnName("IDAreaImpressao");
            this.Property(t => t.ConteudoImpressao).HasColumnName("ConteudoImpressao");
            this.Property(t => t.DtOrdem).HasColumnName("DtOrdem");
            //this.Property(t => t.EnviadoFilaImpressao).HasColumnName("EnviadoFilaImpressao");
            //this.Property(t => t.Conta).HasColumnName("Conta");
            //this.Property(t => t.SAT).HasColumnName("SAT");

            // Relationships
            this.HasRequired(t => t.tbAreaImpressao)
                .WithMany(t => t.tbOrdemImpressaos)
                .HasForeignKey(d => d.IDAreaImpressao);

        }
    }
}
