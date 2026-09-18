using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Owniverse.Shared.Messaging;

namespace Owniverse.Infrastructure.Messaging;

public static class MessagingServiceCollectionExtensions
{
    public static IServiceCollection AddMessaging(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<RabbitMqOptions>()
            .Configure(options => options.Url = configuration[RabbitMqOptions.ConfigurationKey] ?? string.Empty)
            .Validate(options => options.IsValid(),
                "RABBITMQ_URL is required and must be an amqp/amqps URI with host, " +
                "username and password, and without query or fragment. See backend/README.md.")
            .ValidateOnStart();
        services.AddSingleton<RabbitMqConnection>();
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();
        services.AddHealthChecks().AddCheck<RabbitMqHealthCheck>("rabbitmq",
            tags: ["ready"], timeout: TimeSpan.FromSeconds(5));
        return services;
    }
}
