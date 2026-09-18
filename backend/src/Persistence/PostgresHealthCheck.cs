using Microsoft.Extensions.Diagnostics.HealthChecks;
using Npgsql;

namespace Owniverse.Persistence;

internal sealed class PostgresHealthCheck(NpgsqlDataSource dataSource) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
            await using var command = new NpgsqlCommand("SELECT 1", connection);
            command.CommandTimeout = 5;
            await command.ExecuteScalarAsync(cancellationToken);
            return HealthCheckResult.Healthy();
        }
        catch (Exception exception) when (
            exception is NpgsqlException or TimeoutException or OperationCanceledException)
        {
            // Health responses and logs must not expose credentials or server details.
            return HealthCheckResult.Unhealthy("PostgreSQL is unavailable.");
        }
    }
}
