using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbClienteMap : EntityTypeConfiguration<tbCliente>
    {
        public tbClienteMap()
        {
            // Primary Key
            this.HasKey(t => t.IDCliente);

            // Properties
            this.Property(t => t.CodigoERP)
                .HasMaxLength(50);

            this.Property(t => t.NomeCompleto)
                .HasMaxLength(50);

            this.Property(t => t.Documento1)
                .HasMaxLength(50);

            this.Property(t => t.Endereco)
                .HasMaxLength(500);

            this.Property(t => t.EnderecoNumero)
                .HasMaxLength(50);

            this.Property(t => t.Complemento)
                .HasMaxLength(500);

            this.Property(t => t.Bairro)
                .HasMaxLength(500);

            this.Property(t => t.Cidade)
                .HasMaxLength(500);

            this.Property(t => t.Sexo)
                .HasMaxLength(1);

            this.Property(t => t.Email)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("tbCliente");
            this.Property(t => t.IDCliente).HasColumnName("IDCliente");
            this.Property(t => t.CodigoERP).HasColumnName("CodigoERP");
            this.Property(t => t.NomeCompleto).HasColumnName("NomeCompleto");
            this.Property(t => t.Telefone1DDD).HasColumnName("Telefone1DDD");
            this.Property(t => t.Telefone1Numero).HasColumnName("Telefone1Numero");
            this.Property(t => t.Telefone2DDD).HasColumnName("Telefone2DDD");
            this.Property(t => t.Telefone2Numero).HasColumnName("Telefone2Numero");
            this.Property(t => t.Documento1).HasColumnName("Documento1");
            this.Property(t => t.Limite).HasColumnName("Limite");
            this.Property(t => t.Credito).HasColumnName("Credito");
            this.Property(t => t.Bloqueado).HasColumnName("Bloqueado");
            this.Property(t => t.Observacao).HasColumnName("Observacao");
            this.Property(t => t.Endereco).HasColumnName("Endereco");
            this.Property(t => t.EnderecoNumero).HasColumnName("EnderecoNumero");
            this.Property(t => t.Complemento).HasColumnName("Complemento");
            this.Property(t => t.Bairro).HasColumnName("Bairro");
            this.Property(t => t.Cidade).HasColumnName("Cidade");
            this.Property(t => t.IDEstado).HasColumnName("IDEstado");
            this.Property(t => t.CEP).HasColumnName("CEP");
            this.Property(t => t.EnderecoReferencia).HasColumnName("EnderecoReferencia");
            this.Property(t => t.DataNascimento).HasColumnName("DataNascimento");
            this.Property(t => t.Sexo).HasColumnName("Sexo");
            this.Property(t => t.DtInclusao).HasColumnName("DtInclusao");
            this.Property(t => t.Email).HasColumnName("Email");

            // Relationships
            this.HasOptional(t => t.tbEstado)
                .WithMany(t => t.tbClientes)
                .HasForeignKey(d => d.IDEstado);

        }
    }
}
