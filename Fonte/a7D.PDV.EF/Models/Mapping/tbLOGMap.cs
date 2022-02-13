using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    internal class tbLOGMap : EntityTypeConfiguration<tbLOGInformation>
    {
        public tbLOGMap()
        {
            this.HasKey(p => p.IDLOG);

            this.Property(p => p.IDLOG)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(p => p.Data)
                .IsRequired();

            this.Property(p => p.Aplicacao)
                .HasMaxLength(50)
                .IsRequired();

            this.Property(p => p.Versao)
                .HasMaxLength(16)
                .IsRequired();

            this.Property(p => p.Codigo)
                .HasMaxLength(4)
                .IsRequired();

            this.Property(p => p.Titulo)
                .HasMaxLength(200)
                .IsRequired();

            this.Property(p => p.Dados)
                .HasColumnType("text");

            this.ToTable("tbLOG");
        }
    }
}