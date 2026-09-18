using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Owniverse.Persistence;

public static class PersistenceServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<DatabaseOptions>()
            .Configure(options => options.ConnectionString =
                configuration[DatabaseOptions.ConfigurationKey] ?? string.Empty)
            .Validate(options => options.IsValid(),
                "DATABASE_URL is required and must be a valid Npgsql connection string " +
                "with Host, Database and Username. See backend/README.md.")
            .ValidateOnStart();

        services.AddSingleton(provider => NpgsqlDataSource.Create(
            provider.GetRequiredService<IOptions<DatabaseOptions>>().Value.ConnectionString));

        services.AddDbContext<OwniverseDbContext>((provider, options) =>
            options.UseNpgsql(provider.GetRequiredService<NpgsqlDataSource>()));

        services.AddHealthChecks()
            .AddCheck<PostgresHealthCheck>("postgresql", tags: ["ready"],
                timeout: TimeSpan.FromSeconds(5));

        return services;
    }
}
