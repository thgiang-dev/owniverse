using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Owniverse.Shared.Storage;

namespace Owniverse.Infrastructure.Storage;

public static class StorageServiceCollectionExtensions
{
    public static IServiceCollection AddAssetStorage(this IServiceCollection services,
        IConfiguration configuration, string contentRootPath)
    {
        services.AddOptions<AssetStorageOptions>()
            .Configure(options =>
            {
                options.Provider = configuration["ASSET_STORAGE_PROVIDER"] ?? "local";
                options.RootPath = AssetStorageOptions.ResolveRoot(configuration["ASSET_STORAGE_PATH"], contentRootPath);
            })
            .Validate(options => options.Provider == "local",
                "ASSET_STORAGE_PROVIDER must be local. S3 storage is not implemented in this task.")
            .Validate(options => !string.IsNullOrEmpty(options.RootPath),
                "ASSET_STORAGE_PATH is required and must resolve to a directory outside compiled application output.")
            .ValidateOnStart();
        services.AddSingleton<IAssetStorage, LocalFileAssetStorage>();
        services.AddHealthChecks().AddCheck<AssetStorageHealthCheck>("asset-storage",
            tags: ["ready"], timeout: TimeSpan.FromSeconds(5));
        return services;
    }
}
