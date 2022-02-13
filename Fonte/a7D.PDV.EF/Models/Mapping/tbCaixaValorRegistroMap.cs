using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbCaixaValorRegistroMap : EntityTypeConfiguration<tbCaixaValorRegistro>
    {
        public tbCaixaValorRegistroMap()
        {
            // Primary Key
            this.HasKey(t => t.IDCaixaValorRegistro);

            // Properties
            // Table & Column Mappings
            this.ToTable("tbCaixaValorRegistro");
            this.Property(t => t.IDCaixaValorRegistro).HasColumnName("IDCaixaValorRegistro");
            this.Property(t => t.IDCaixa).HasColumnName("IDCaixa");
            this.Property(t => t.IDTipoPagamento).HasColumnName("IDTipoPagamento");
            this.Property(t => t.ValorAbertura).HasColumnName("ValorAbertura");
            this.Property(t => t.ValorFechamento).HasColumnName("ValorFechamento");

            // Relationships
            this.HasRequired(t => t.tbCaixa)
                .WithMany(t => t.tbCaixaValorRegistroes)
                .HasForeignKey(d => d.IDCaixa);
            this.HasRequired(t => t.tbTipoPagamento)
                .WithMany(t => t.tbCaixaValorRegistroes)
                .HasForeignKey(d => d.IDTipoPagamento);

        }
    }
}
