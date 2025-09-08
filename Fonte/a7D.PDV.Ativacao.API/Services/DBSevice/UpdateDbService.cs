namespace a7D.PDV.Ativacao.API.Services.DBSevice;

public class UpdateDbService
{
    readonly IDbCommandQueue _queue;

    public UpdateDbService(IDbCommandQueue queue)
    {
        _queue = queue;
    }

    public async ValueTask RequestChangesAsync(string sql, params object[] parameters)
        => await _queue.QueueAsync(new DbCommandItem(sql, parameters));
}
