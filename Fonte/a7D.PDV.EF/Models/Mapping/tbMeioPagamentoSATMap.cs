using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbMeioPagamentoSATMap : EntityTypeConfiguration<tbMeioPagamento>
    {
        public tbMeioPagamentoSATMap()
        {
            // Primary Key
            this.HasKey(t => t.IDMeioPagamentoSAT);

            // Properties
            this.Property(t => t.Codigo)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.Descricao)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("tbMeioPagamentoSAT");
            this.Property(t => t.IDMeioPagamentoSAT).HasColumnName("IDMeioPagamentoSAT");
            this.Property(t => t.Codigo).HasColumnName("Codigo");
            this.Property(t => t.Descricao).HasColumnName("Descricao");
        }
    }
}
