using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.EF.Models.Mapping
{
    public class tbFaturaPixContaMap : EntityTypeConfiguration<tbFaturaPixConta>
    {
        public tbFaturaPixContaMap()
        {
            this.HasKey(t => t.IDFaturaPixConta);
            this.Property(t => t.CodigoFatura).HasMaxLength(50);
            this.Property(t => t.Status).HasMaxLength(10);

            this.ToTable("tbFaturaPixConta");

            this.HasRequired(t => t.tbPedido)
                .WithMany(t => t.FaturasPixConta)
                .HasForeignKey(d => d.IDPedido);
        }
    }
}
