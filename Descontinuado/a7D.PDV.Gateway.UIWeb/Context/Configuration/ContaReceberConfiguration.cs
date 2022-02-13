using a7D.PDV.Gateway.UIWeb.Interface;
using a7D.PDV.Gateway.UIWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.ComponentModel.Composition;

namespace a7D.PDV.Gateway.UIWeb.Context.Configuration
{
    [Export(typeof(IEntityConfiguration))]
    public class ContaReceberConfiguration : EntityTypeConfiguration<ContaReceber>, IEntityConfiguration
    {

        public ContaReceberConfiguration()
        {
            ToTable("ContasReceber");
            HasRequired(cr => cr.Cliente);
        }

        public void AddConfiguration(ConfigurationRegistrar registrar)
        {
            registrar.Add(this);
        }
    }
}