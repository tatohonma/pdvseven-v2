using a7D.PDV.Gateway.UIWeb.Context.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using a7D.PDV.Gateway.UIWeb.Models;

namespace a7D.PDV.Gateway.UIWeb.Context
{
    public class GatewayContext : DbContext
    {
        public DbSet<ContaReceber> ContasReceber { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ItemPedido> ItensPedido { get; set; }
        public DbSet<InteracaoPagarMe> InteracoesPagarMe { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var contextConfiguration = new ContextConfiguration();
            var catalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            var container = new CompositionContainer(catalog);
            container.ComposeParts(contextConfiguration);

            foreach (var configuration in contextConfiguration.Configurations)
            {
                configuration.AddConfiguration(modelBuilder.Configurations);
            }

            base.OnModelCreating(modelBuilder);
        }

        public T UnProxy<T>(T proxyObject) where T : class
        {
            var proxyCreationEnabled = this.Configuration.ProxyCreationEnabled;
            try
            {
                this.Configuration.ProxyCreationEnabled = false;
                T poco = this.Entry(proxyObject).CurrentValues.ToObject() as T;
                return poco;
            }
            finally
            {
                this.Configuration.ProxyCreationEnabled = proxyCreationEnabled;
            }
        }

        public IEnumerable<T> UnProxy<T>(List<T> proxyObjects) where T : class
        {
            var proxyCreationEnabled = this.Configuration.ProxyCreationEnabled;
            try
            {
                foreach (var obj in proxyObjects)
                    yield return UnProxy(obj);
            }
            finally
            {
                this.Configuration.ProxyCreationEnabled = proxyCreationEnabled;
            }
        }
    }
}