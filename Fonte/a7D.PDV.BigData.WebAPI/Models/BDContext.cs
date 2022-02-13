using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Dynamic;

namespace a7D.PDV.BigData.WebAPI.Models
{
    // https://docs.microsoft.com/pt-br/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/migrations-and-deployment-with-the-entity-framework-in-an-asp-net-mvc-application
    // EntityFramework\enable-migrations
    // EntityFramework\add-migration InitialCreate
    // EntityFramework\update-database

    public class BDContext : DbContext
    {
        public DbSet<Entidade> Entidades { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Produto> Produtos { get; set; }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<TipoPagamento> TipoPagamento { get; set; }

        public DbSet<Pedido> Pedidos { get; set; }

        public DbSet<PedidoProduto> PedidoProdutos { get; set; }

        public DbSet<PedidoPagamento> PedidoPagamentos { get; set; }

        public DbSet<Querys> Querys { get; set; }

        public DbSet<ChannelUser> ChannelUsers { get; set; }

        static BDContext()
        {
            Database.SetInitializer<BDContext>(null);
        }

        public BDContext() : base("Name=connectionBDString")
        {
            this.Configuration.ProxyCreationEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
        }

        // https://stackoverflow.com/questions/26749429/anonymous-type-result-from-sql-query-execution-entity-framework
        public IEnumerable<dynamic> DynamicListFromSql(string sql, IEnumerable<KeyValuePair<string, object>> prms)
        {
            using (var command = Database.Connection.CreateCommand())
            {
                command.CommandText = sql;
                if (command.Connection.State != ConnectionState.Open)
                    command.Connection.Open();

                if (prms != null)
                {
                    foreach (var p in prms)
                    {
                        var dbParameter = command.CreateParameter();
                        dbParameter.ParameterName = "@" + p.Key;
                        dbParameter.Value = p.Value;
                        command.Parameters.Add(dbParameter);
                    }
                }

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var row = new ExpandoObject() as IDictionary<string, object>;
                        for (var fieldCount = 0; fieldCount < reader.FieldCount; fieldCount++)
                        {
                            row.Add(reader.GetName(fieldCount), reader[fieldCount]);
                        }
                        yield return row;
                    }
                }
            }
        }
    }
}