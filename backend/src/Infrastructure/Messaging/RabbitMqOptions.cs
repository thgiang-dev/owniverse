using RabbitMQ.Client;

namespace Owniverse.Infrastructure.Messaging;

public sealed class RabbitMqOptions
{
    public const string ConfigurationKey = "RABBITMQ_URL";
    public string Url { get; set; } = string.Empty;

    internal bool IsValid()
    {
        if (string.IsNullOrWhiteSpace(Url) || Url.Any(char.IsWhiteSpace)
            || !Uri.TryCreate(Url, UriKind.Absolute, out var uri)
            || uri.Scheme is not ("amqp" or "amqps")
            || string.IsNullOrEmpty(uri.Host) || uri.Port == 0
            || uri.Query.Length != 0 || uri.Fragment.Length != 0)
        {
            return false;
        }

        var credentials = uri.UserInfo.Split(':');
        if (credentials.Length != 2 || credentials.Any(string.IsNullOrEmpty))
        {
            return false;
        }

        try
        {
            _ = new ConnectionFactory { Uri = uri };
            return true;
        }
        catch (Exception exception) when (exception is ArgumentException or UriFormatException)
        {
            return false;
        }
    }
}
