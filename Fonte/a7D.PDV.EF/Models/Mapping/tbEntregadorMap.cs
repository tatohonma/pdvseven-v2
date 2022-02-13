using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbEntregadorMap : EntityTypeConfiguration<tbEntregador>
    {
        public tbEntregadorMap()
        {
            // Primary Key
            this.HasKey(t => t.IDEntregador);

            // Properties
            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("tbEntregador");
            this.Property(t => t.IDEntregador).HasColumnName("IDEntregador");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.Ativo).HasColumnName("Ativo");
            this.Property(t => t.Excluido).HasColumnName("Excluido");
            this.Property(t => t.IDGateway).HasColumnName("IDGateway").IsOptional();
        }
    }
}
