using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace a7D.PDV.EF.Models.Mapping
{
    internal class tbGatewayMap : EntityTypeConfiguration<tbGateway>
    {
        public tbGatewayMap()
        {
            this.HasKey(p => p.IDGateway);

            this.Property(p => p.IDGateway)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None); // Não é auto incementeo

            this.Property(t => t.Nome)
                .HasMaxLength(50)
                .IsRequired();

            //this.Ignore(t => t.id);
            //this.Ignore(t => t.name);
            //this.Ignore(t => t.Tipo);

            this.ToTable("tbGateway");
        }
    }
}