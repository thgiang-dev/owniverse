namespace Owniverse.Shared.Messaging;

// Transport boundary only. The caller owns serialization and destination selection.
public interface IMessagePublisher
{
    Task PublishAsync(string destination, ReadOnlyMemory<byte> body,
        string contentType, CancellationToken cancellationToken = default);
}
