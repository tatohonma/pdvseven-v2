using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    internal class tbTamanhoPacoteMAP : EntityTypeConfiguration<tbTamanhoPacote>
    {
        public tbTamanhoPacoteMAP()
        {
            this.HasKey(p => p.IDTamanhoPacote);

            this.Property(p => p.IDTamanhoPacote)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None); // Não é auto incementeo

            this.Property(t => t.Nome)
                .HasMaxLength(50)
                .IsRequired();

            this.ToTable("tbTamanhoPacote");
        }
    }
}