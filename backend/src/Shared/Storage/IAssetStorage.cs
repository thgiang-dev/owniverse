namespace Owniverse.Shared.Storage;

/// <summary>Binary object operations using logical, server-generated storage keys.</summary>
public interface IAssetStorage
{
    /// <summary>Streams from the current position, leaves input open, and never overwrites an existing key.</summary>
    Task UploadAsync(string storageKey, Stream content, CancellationToken cancellationToken = default);

    /// <summary>Returns a caller-owned stream; throws FileNotFoundException for a missing object.</summary>
    Task<Stream> OpenReadAsync(string storageKey, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(string storageKey, CancellationToken cancellationToken = default);

    /// <summary>Deletes only the physical object. A missing object is a no-op.</summary>
    Task DeleteAsync(string storageKey, CancellationToken cancellationToken = default);
}
