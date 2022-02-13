using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbUsuarioMap : EntityTypeConfiguration<tbUsuario>
    {
        public tbUsuarioMap()
        {
            // Primary Key
            this.HasKey(t => t.IDUsuario);

            // Properties
            this.Property(t => t.CodigoERP)
                .HasMaxLength(50);

            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Login)
                .HasMaxLength(50);

            this.Property(t => t.Senha)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("tbUsuario");
            this.Property(t => t.IDUsuario).HasColumnName("IDUsuario");
            this.Property(t => t.CodigoERP).HasColumnName("CodigoERP");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.Login).HasColumnName("Login");
            this.Property(t => t.Senha).HasColumnName("Senha");
            this.Property(t => t.PermissaoAdm).HasColumnName("PermissaoAdm");
            this.Property(t => t.PermissaoCaixa).HasColumnName("PermissaoCaixa");
            this.Property(t => t.PermissaoGarcom).HasColumnName("PermissaoGarcom");
            this.Property(t => t.PermissaoGerente).HasColumnName("PermissaoGerente");
            this.Property(t => t.Ativo).HasColumnName("Ativo");
            this.Property(t => t.DtUltimaAlteracao).HasColumnName("DtUltimaAlteracao");
            this.Property(t => t.Excluido).HasColumnName("Excluido");
        }
    }
}
