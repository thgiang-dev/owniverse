using Microsoft.Extensions.Diagnostics.HealthChecks;
using Owniverse.Shared.Storage;

namespace Owniverse.Infrastructure.Storage;

internal sealed class AssetStorageHealthCheck(IAssetStorage storage) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var key = $"health-{Guid.NewGuid():N}.tmp";
        var uploaded = false;
        try
        {
            try
            {
                using var content = new MemoryStream(new byte[] { 42 });
                await storage.UploadAsync(key, content, cancellationToken);
                uploaded = true;
                await using var stored = await storage.OpenReadAsync(key, cancellationToken);
                var actual = new byte[1];
                await stored.ReadExactlyAsync(actual, cancellationToken);
                if (actual[0] != 42)
                {
                    return HealthCheckResult.Unhealthy("Asset storage integrity check failed.");
                }
            }
            finally
            {
                if (uploaded)
                {
                    await storage.DeleteAsync(key, CancellationToken.None);
                }
            }
            return HealthCheckResult.Healthy();
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException
            or ArgumentException or NotSupportedException or OperationCanceledException)
        {
            return HealthCheckResult.Unhealthy("Asset storage is unavailable.");
        }
    }
}
