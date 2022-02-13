using a7D.PDV.Gateway.UIWeb.Interface;
using a7D.PDV.Gateway.UIWeb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration.Configuration;

namespace a7D.PDV.Gateway.UIWeb.Context.Configuration
{
    [Export(typeof(IEntityConfiguration))]
    public class PedidoConfiguration : EntityTypeConfiguration<Pedido>, IEntityConfiguration
    {
        public PedidoConfiguration()
        {
            ToTable("Pedidos");
        }
        public void AddConfiguration(ConfigurationRegistrar registrar)
        {
            registrar.Add(this);
        }
    }
}