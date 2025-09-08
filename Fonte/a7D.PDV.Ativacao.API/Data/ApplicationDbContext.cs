using System.Text;
using a7D.PDV.Ativacao.API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace a7D.PDV.Ativacao.API.Data;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options, bool enableLog = false
    ) : IdentityDbContext<AppUser>(options)
{
    StringBuilder? _sbLog;
    public string? LogSql => _sbLog?.ToString();

    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Activation> Activations => Set<Activation>();
    public DbSet<Reseller> Resellers => Set<Reseller>();
    public DbSet<Pdv> PdVs => Set<Pdv>();
    public DbSet<PdvType> PdvTypes => Set<PdvType>();
    public DbSet<AppUser> User => Set<AppUser>();
    public DbSet<Mensagem> Mensagems => Set<Mensagem>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (enableLog)
        {
            _sbLog = new StringBuilder();
            optionsBuilder.LogTo(s =>
            {
                if (_sbLog.Length > 1000)
                {
                    _sbLog.Clear();
                    _sbLog.AppendLine(s);
                }
            });
        }
    }

    public override int SaveChanges()
    {
        var entires = ChangeTracker
            .Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);


        foreach (var entire in entires)
        {
            ((BaseModel)entire.Entity).CreatedAt = DateTime.Now;

            if (entire.State == EntityState.Modified)
            {
                ((BaseModel)entire.Entity).UpdatedAt = DateTime.Now;
            }
        }
        return base.SaveChanges();
    }
}
