using System.Threading.Channels;
using a7D.PDV.Ativacao.API.Services.DBSevice;

public sealed class DbCommandQueue : IDbCommandQueue
{
    private readonly Channel<DbCommandItem> _channel;

    public DbCommandQueue()
    {
        _channel = Channel.CreateUnbounded<DbCommandItem>(new UnboundedChannelOptions
        {
            SingleReader = true,
            SingleWriter = false
        });
    }

    public ValueTask QueueAsync(DbCommandItem item, CancellationToken ct = default)
        => _channel.Writer.WriteAsync(item, ct);

    public IAsyncEnumerable<DbCommandItem> ReadAllAsync(CancellationToken ct)
        => _channel.Reader.ReadAllAsync(ct);
}
