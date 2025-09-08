using a7D.PDV.Ativacao.API.Data;
using a7D.PDV.Ativacao.API.Services.DBSevice;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public sealed class DbCommandWorker : BackgroundService
{
    readonly ILogger<DbCommandWorker> _logger;
    readonly IServiceProvider _sp;
    readonly DbCommandQueue _queue;

    public DbCommandWorker(
        ILogger<DbCommandWorker> logger,
        IServiceProvider sp,
        DbCommandQueue queue)
    {
        _logger = logger;
        _sp     = sp;
        _queue  = queue;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("DbCommandWorker iniciado.");

        await foreach (var item in _queue.ReadAllAsync(stoppingToken))
        {
            try
            {
                using var scope = _sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var affected = await db.Database.ExecuteSqlRawAsync(item.Sql, item.Parameters, stoppingToken);
                _logger.LogDebug("SQL executado. Linhas afetadas: {n}", affected);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao executar comando SQL em background");
            }
        }

        _logger.LogInformation("DbCommandWorker finalizado.");
    }
}
