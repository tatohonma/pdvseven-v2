using Microsoft.EntityFrameworkCore;

namespace a7D.PDV.Iaago.WebApp.Models
{
    // https://docs.microsoft.com/pt-br/ef/core/get-started/install/
    // enable-migrations
    // add-migration InitialCreate
    // update-database

    public class IaagoContext : DbContext
    {
        public DbSet<Pergunta> Perguntas { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Variaveis> Variaveis { get; set; }

        public IaagoContext(DbContextOptions<IaagoContext> options)
            : base(options)
        {
        }
    }
}