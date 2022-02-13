using a7D.PDV.Ativacao.API.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace a7D.PDV.Ativacao.API.Context
{
    public class AtivacaoContext : DbContext
    {
        private StringBuilder sbLog;
        public string LogSQL => sbLog?.ToString();

        public AtivacaoContext(bool log = false)
        {
            Database.SetInitializer<AtivacaoContext>(null);
            if(log)
            {
                sbLog = new StringBuilder();
                Database.Log = s =>
                {
                    if (sbLog.Length > 10000)
                        sbLog.Clear();

                    sbLog.AppendLine(s);
                };
            }
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Entities.Ativacao> Ativacoes { get; set; }
        public DbSet<Revenda> Revendas { get; set; }
        public DbSet<Entities.PDV> PDVs { get; set; }
        public DbSet<TipoPDV> TipoPDVs { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Mensagem> Mensagens { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.PDV>().HasRequired(pdv => pdv.Ativacao);
            modelBuilder.Entity<Entities.PDV>().HasRequired(pdv => pdv.TipoPDV);
            modelBuilder.Entity<Entities.Ativacao>().HasRequired(a => a.Cliente);
            modelBuilder.Entity<Cliente>().HasRequired(c => c.Revenda);
            modelBuilder.Entity<Mensagem>().HasRequired(m => m.Ativacao);
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