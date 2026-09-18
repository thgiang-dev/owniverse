using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Owniverse.Infrastructure.Messaging;
using Owniverse.Persistence;
using Owniverse.Shared.Messaging;
using RabbitMQ.Client;

if (args is ["storage-tests"])
{
    return await AssetStorageChecks.RunAsync();
}

// Test harness only. Dependency probes are read-only; publisher smoke uses a temporary queue.
if (args.Length != 1 || args[0] is not ("postgresql" or "rabbitmq" or "publisher" or "publisher-unavailable"))
{
    Console.Error.WriteLine("Usage: InfrastructureSmoke postgresql|rabbitmq|publisher|publisher-unavailable");
    return 2;
}

var configuration = new ConfigurationBuilder().AddEnvironmentVariables().Build();
var services = new ServiceCollection();
services.AddLogging();
services.AddPersistence(configuration);
services.AddMessaging(configuration);
await using var provider = services.BuildServiceProvider();
using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(15));
try
{
    if (args[0] is "postgresql" or "rabbitmq")
    {
        var report = await provider.GetRequiredService<HealthCheckService>()
            .CheckHealthAsync(registration => registration.Name == args[0], timeout.Token);
        Console.WriteLine($"{args[0]}: {report.Status}");
        return report.Status == HealthStatus.Healthy ? 0 : 1;
    }

    var publisher = provider.GetRequiredService<IMessagePublisher>();
    var body = new byte[] { 1, 2, 3 }; // Opaque test bytes, not a Job message.
    if (args[0] == "publisher-unavailable")
    {
        try
        {
            await publisher.PublishAsync("infrastructure-smoke", body, "application/octet-stream", timeout.Token);
            throw new InvalidOperationException("Unavailable broker must not confirm publication.");
        }
        catch (MessagePublishException)
        {
            Console.WriteLine("Publisher reports unavailable broker: PASS");
            return 0;
        }
    }

    var url = provider.GetRequiredService<IOptions<RabbitMqOptions>>().Value.Url;
    var factory = new ConnectionFactory { Uri = new Uri(url), AutomaticRecoveryEnabled = false };
    await using var connection = await factory.CreateConnectionAsync(timeout.Token);
    await using var channel = await connection.CreateChannelAsync(cancellationToken: timeout.Token);
    var queue = await channel.QueueDeclareAsync(queue: string.Empty, durable: false,
        exclusive: true, autoDelete: true, cancellationToken: timeout.Token);
    await publisher.PublishAsync(queue.QueueName, body, "application/octet-stream", timeout.Token);
    var message = await channel.BasicGetAsync(queue.QueueName, autoAck: true, timeout.Token);
    if (message is null || !message.Body.Span.SequenceEqual(body)
        || !message.BasicProperties.Persistent || message.BasicProperties.ContentType != "application/octet-stream")
    {
        throw new InvalidOperationException("Publisher round trip failed.");
    }
    try
    {
        await publisher.PublishAsync($"owniverse-smoke-missing-{Guid.NewGuid():N}", body,
            "application/octet-stream", timeout.Token);
        throw new InvalidOperationException("Unroutable publication must fail.");
    }
    catch (MessagePublishException)
    {
        Console.WriteLine("Publisher round trip and unroutable rejection: PASS");
    }
    return 0;
}
catch (Exception exception)
{
    Console.Error.WriteLine($"Infrastructure smoke failed ({exception.GetType().Name}); connection details omitted.");
    return 1;
}
