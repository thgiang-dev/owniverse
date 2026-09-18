using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Owniverse.Infrastructure.Messaging;

internal sealed class RabbitMqConnection(IOptions<RabbitMqOptions> options) : IAsyncDisposable
{
    private readonly SemaphoreSlim gate = new(1, 1);
    private IConnection? connection;

    public async Task<IConnection> GetAsync(CancellationToken cancellationToken)
    {
        await gate.WaitAsync(cancellationToken);
        try
        {
            if (connection is { IsOpen: true })
            {
                return connection;
            }

            if (connection is not null)
            {
                await connection.DisposeAsync();
            }

            var factory = new ConnectionFactory
            {
                Uri = new Uri(options.Value.Url),
                ClientProvidedName = "owniverse-api",
                RequestedConnectionTimeout = TimeSpan.FromSeconds(5),
                HandshakeContinuationTimeout = TimeSpan.FromSeconds(5),
                ContinuationTimeout = TimeSpan.FromSeconds(5),
                AutomaticRecoveryEnabled = false
            };
            connection = await factory.CreateConnectionAsync(cancellationToken);
            return connection;
        }
        finally
        {
            gate.Release();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (connection is not null)
        {
            await connection.DisposeAsync();
        }
        gate.Dispose();
    }
}
