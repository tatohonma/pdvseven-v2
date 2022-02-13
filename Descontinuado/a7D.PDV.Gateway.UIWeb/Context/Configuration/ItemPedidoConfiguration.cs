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
    public class ItemPedidoConfiguration : EntityTypeConfiguration<ItemPedido>, IEntityConfiguration
    {
        public ItemPedidoConfiguration()
        {
            ToTable("ItensPedido");
            Property(ip => ip.IdPedido).IsRequired();
            Property(ip => ip.IdContaReceber).IsRequired();
        }
        public void AddConfiguration(ConfigurationRegistrar registrar)
        {
            registrar.Add(this);
        }
    }
}