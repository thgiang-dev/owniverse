using Owniverse.Shared.Messaging;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace Owniverse.Infrastructure.Messaging;

internal sealed class RabbitMqPublisher(RabbitMqConnection connection)
    : IMessagePublisher, IAsyncDisposable
{
    private readonly SemaphoreSlim gate = new(1, 1);
    private IChannel? channel;

    public async Task PublishAsync(string destination, ReadOnlyMemory<byte> body,
        string contentType, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(destination);
        ArgumentException.ThrowIfNullOrWhiteSpace(contentType);
        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeout.CancelAfter(TimeSpan.FromSeconds(10));
        await gate.WaitAsync(timeout.Token);
        try
        {
            var broker = await connection.GetAsync(timeout.Token);
            if (channel is not { IsOpen: true })
            {
                if (channel is not null)
                {
                    await channel.DisposeAsync();
                }
                channel = await broker.CreateChannelAsync(
                    new CreateChannelOptions(publisherConfirmationsEnabled: true,
                        publisherConfirmationTrackingEnabled: true), timeout.Token);
            }

            // The destination must already exist. Do not declare business topology here.
            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: destination,
                mandatory: true, basicProperties: new BasicProperties
                {
                    Persistent = true,
                    ContentType = contentType
                }, body: body, cancellationToken: timeout.Token);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception) when (exception is RabbitMQClientException
            or IOException or TimeoutException or OperationCanceledException)
        {
            // No automatic resend: an interrupted confirmation may have an unknown outcome.
            throw new MessagePublishException();
        }
        finally
        {
            gate.Release();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (channel is not null)
        {
            await channel.DisposeAsync();
        }
        gate.Dispose();
    }
}
