using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbCaixaMap : EntityTypeConfiguration<tbCaixa>
    {
        public tbCaixaMap()
        {
            // Primary Key
            this.HasKey(t => t.IDCaixa);

            // Properties
            // Table & Column Mappings
            this.ToTable("tbCaixa");
            this.Property(t => t.IDCaixa).HasColumnName("IDCaixa");
            this.Property(t => t.IDPDV).HasColumnName("IDPDV");
            this.Property(t => t.IDUsuario).HasColumnName("IDUsuario");
            this.Property(t => t.IDFechamento).HasColumnName("IDFechamento");
            this.Property(t => t.DtAbertura).HasColumnName("DtAbertura");
            this.Property(t => t.DtFechamento).HasColumnName("DtFechamento");
            this.Property(t => t.SincERP).HasColumnName("SincERP");

            // Relationships
            this.HasOptional(t => t.tbFechamento)
                .WithMany(t => t.tbCaixas)
                .HasForeignKey(d => d.IDFechamento);
            this.HasRequired(t => t.tbPDV)
                .WithMany(t => t.tbCaixas)
                .HasForeignKey(d => d.IDPDV);
            this.HasOptional(t => t.tbUsuario)
                .WithMany(t => t.tbCaixas)
                .HasForeignKey(d => d.IDUsuario);

        }
    }
}
