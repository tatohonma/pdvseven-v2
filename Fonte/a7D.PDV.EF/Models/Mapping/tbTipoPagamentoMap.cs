using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbTipoPagamentoMap : EntityTypeConfiguration<tbTipoPagamento>
    {
        public tbTipoPagamentoMap()
        {
            // Primary Key
            this.HasKey(t => t.IDTipoPagamento);

            // Properties
            this.Property(t => t.CodigoImpressoraFiscal)
                .HasMaxLength(50);

            this.Property(t => t.CodigoERP)
                .HasMaxLength(50);

            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("tbTipoPagamento");
            this.Property(t => t.IDTipoPagamento).HasColumnName("IDTipoPagamento");
            this.Property(t => t.CodigoImpressoraFiscal).HasColumnName("CodigoImpressoraFiscal");
            //this.Property(t => t.CodigoERP).HasColumnName("CodigoERP");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.RegistrarValores).HasColumnName("RegistrarValores");
            this.Property(t => t.PrazoCredito).HasColumnName("PrazoCredito");
            this.Property(t => t.Ativo).HasColumnName("Ativo");
            this.Property(t => t.IDMeioPagamentoSAT).HasColumnName("IDMeioPagamentoSAT");
            this.Property(t => t.IDGateway).HasColumnName("IDGateway");

            // Relationships
            this.HasOptional(t => t.tbMeioPagamentoSAT)
                .WithMany(t => t.tbTipoPagamentoes)
                .HasForeignKey(d => d.IDMeioPagamentoSAT);

        }
    }
}
