using Npgsql;

namespace Owniverse.Persistence;

public sealed class DatabaseOptions
{
    public const string ConfigurationKey = "DATABASE_URL";

    public string ConnectionString { get; set; } = string.Empty;

    internal bool IsValid()
    {
        if (string.IsNullOrWhiteSpace(ConnectionString))
        {
            return false;
        }

        try
        {
            var connection = new NpgsqlConnectionStringBuilder(ConnectionString);
            return !string.IsNullOrWhiteSpace(connection.Host)
                && !string.IsNullOrWhiteSpace(connection.Database)
                && !string.IsNullOrWhiteSpace(connection.Username);
        }
        catch (ArgumentException)
        {
            // Never include the supplied value or parser exception in validation errors.
            return false;
        }
    }
}
