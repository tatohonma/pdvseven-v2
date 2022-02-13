using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    internal class PesquisaMAP : EntityTypeConfiguration<tbPesquisa>
    {
        public PesquisaMAP()
        {
            this.HasKey(p => p.IDPesquisa);

            this.Property(p => p.IDPesquisa)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(p => p.Data)
                .IsRequired();

            this.Property(p => p.Valor)
                .IsRequired();

            this.ToTable("tbPesquisa");
        }
    }
}