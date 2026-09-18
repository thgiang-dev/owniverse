using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace Owniverse.Infrastructure.Messaging;

internal sealed class RabbitMqHealthCheck(RabbitMqConnection connection) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var broker = await connection.GetAsync(cancellationToken);
            // Opening a channel requires a broker response, unlike a cached IsOpen flag.
            await using var channel = await broker.CreateChannelAsync(cancellationToken: cancellationToken);
            await channel.CloseAsync(cancellationToken: cancellationToken);
            return HealthCheckResult.Healthy();
        }
        catch (Exception exception) when (exception is RabbitMQClientException
            or IOException or TimeoutException or OperationCanceledException)
        {
            return HealthCheckResult.Unhealthy("RabbitMQ is unavailable.");
        }
    }
}
