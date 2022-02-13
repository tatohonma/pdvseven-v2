using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    internal class tbVersaoMap : EntityTypeConfiguration<tbVersao>
    {
        public tbVersaoMap()
        {
            this.HasKey(p => p.IDVersao);

            this.Property(p => p.IDVersao)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Versao)
                .HasMaxLength(50)
                .IsRequired();

            this.ToTable("tbVersao");
        }
    }
}