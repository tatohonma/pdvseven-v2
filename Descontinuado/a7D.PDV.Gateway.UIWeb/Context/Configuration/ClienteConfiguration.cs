using a7D.PDV.Gateway.UIWeb.Interface;
using a7D.PDV.Gateway.UIWeb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace a7D.PDV.Gateway.UIWeb.Context.Configuration
{
    [Export(typeof(IEntityConfiguration))]
    public class ClienteConfiguration : EntityTypeConfiguration<Cliente>, IEntityConfiguration
    {
        public ClienteConfiguration()
        {
            ToTable("Clientes");
            Property(c => c.IdBroker)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_UniqueIdBroker", 1) { IsUnique = true }
                    )
                );
        }

        public void AddConfiguration(ConfigurationRegistrar registrar)
        {
            registrar.Add(this);
        }
    }
}