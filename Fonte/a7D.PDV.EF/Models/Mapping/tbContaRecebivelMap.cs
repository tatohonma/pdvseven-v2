using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    internal class tbContaRecebivelMap : EntityTypeConfiguration<tbContaRecebivel>
    {
        public tbContaRecebivelMap()
        {
            this.HasKey(p => p.IDContaRecebivel);

            this.Property(p => p.IDContaRecebivel)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Nome)
                .HasMaxLength(50)
                .IsRequired();

            this.Property(t => t.CodigoIntegracao)
                .HasMaxLength(50);

            this.ToTable("tbContaRecebivel");
        }
    }
}