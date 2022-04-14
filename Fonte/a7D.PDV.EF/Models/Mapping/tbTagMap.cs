using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbTagMap : EntityTypeConfiguration<tbTag>
    {
        public tbTagMap()
        {
            this.ToTable("tbTag");

            // Primary Key
            this.HasKey(t => t.IDTag);

            this.Property(t => t.IDTag)
                .HasColumnName("IDTag");
            this.Property(t => t.GUIDIdentificacao)
                .HasColumnName("GUIDIdentificacao")
                .HasMaxLength(50)
                .IsRequired();
            this.Property(t => t.Chave)
                .HasColumnName("Chave")
                .HasMaxLength(50)
                .IsRequired();
            this.Property(t => t.Valor)
                .HasColumnName("Valor")
                .HasMaxLength(5000);
            this.Property(t => t.DtInclusao)
                .HasColumnName("DtInclusao");
        }
    }
}
