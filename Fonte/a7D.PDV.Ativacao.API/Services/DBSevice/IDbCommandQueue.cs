namespace a7D.PDV.Ativacao.API.Services.DBSevice;

public sealed record DbCommandItem(
    string Sql,
    object[] Parameters);

public interface IDbCommandQueue
{
    ValueTask QueueAsync(DbCommandItem item, CancellationToken ct = default);
}
