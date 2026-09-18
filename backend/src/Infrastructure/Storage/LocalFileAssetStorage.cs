using Microsoft.Extensions.Options;
using Owniverse.Shared.Storage;

namespace Owniverse.Infrastructure.Storage;

public sealed class LocalFileAssetStorage(IOptions<AssetStorageOptions> options) : IAssetStorage
{
    private readonly string root = options.Value.RootPath;
    private static readonly StringComparison PathComparison = OperatingSystem.IsWindows()
        ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

    public async Task UploadAsync(string storageKey, Stream content, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(content);
        if (!content.CanRead)
        {
            throw new ArgumentException("Content must be readable.", nameof(content));
        }
        cancellationToken.ThrowIfCancellationRequested();
        var target = ResolveKey(storageKey);
        var parent = Path.GetDirectoryName(target)!;
        EnsureDirectory(parent);
        // Hidden staging names cannot be addressed through a valid logical key.
        var temporary = Path.Combine(parent, $".upload-{Guid.NewGuid():N}.tmp");
        var created = false;
        try
        {
            AssertNoLinks(temporary);
            await using (var output = new FileStream(temporary, FileMode.CreateNew, FileAccess.Write,
                FileShare.None, 81920, FileOptions.Asynchronous | FileOptions.SequentialScan))
            {
                created = true;
                await content.CopyToAsync(output, 81920, cancellationToken);
                await output.FlushAsync(cancellationToken);
            }
            cancellationToken.ThrowIfCancellationRequested();
            AssertNoLinks(target);
            AssertNoLinks(temporary);
            File.Move(temporary, target, overwrite: false);
        }
        finally
        {
            if (created)
            {
                AssertNoLinks(temporary);
                File.Delete(temporary);
            }
        }
    }

    public Task<Stream> OpenReadAsync(string storageKey, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var target = ResolveKey(storageKey);
        try
        {
            return Task.FromResult<Stream>(new FileStream(target, FileMode.Open, FileAccess.Read,
                FileShare.Read, 81920, FileOptions.Asynchronous | FileOptions.SequentialScan));
        }
        catch (DirectoryNotFoundException)
        {
            throw new FileNotFoundException("The storage object does not exist.");
        }
    }

    public Task<bool> ExistsAsync(string storageKey, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var attributes = Attributes(ResolveKey(storageKey));
        return Task.FromResult(attributes is not null && !attributes.Value.HasFlag(FileAttributes.Directory));
    }

    public Task DeleteAsync(string storageKey, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var target = ResolveKey(storageKey);
        if (Attributes(target) is not null)
        {
            File.Delete(target);
        }
        return Task.CompletedTask;
    }

    private string ResolveKey(string storageKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(storageKey);
        var segments = storageKey.Split('/');
        foreach (var segment in segments)
        {
            if (segment.Length is 0 or > 255 || !char.IsAsciiLetterOrDigit(segment[0])
                || segment.EndsWith('.')
                || segment.Any(c => !char.IsAsciiLetterOrDigit(c) && c is not ('.' or '_' or '-'))
                || IsDeviceName(segment))
            {
                throw new ArgumentException("Invalid logical storage key.", nameof(storageKey));
            }
        }
        var target = Path.GetFullPath(Path.Combine(root, Path.Combine(segments)));
        if (!target.StartsWith(root + Path.DirectorySeparatorChar, PathComparison))
        {
            throw new ArgumentException("Storage key escapes the configured root.", nameof(storageKey));
        }
        AssertNoLinks(target);
        return target;
    }

    private static bool IsDeviceName(string segment)
    {
        var name = segment.Split('.')[0].ToUpperInvariant();
        return name is "CON" or "PRN" or "AUX" or "NUL"
            || (name.Length == 4 && (name.StartsWith("COM") || name.StartsWith("LPT"))
                && name[3] is >= '1' and <= '9');
    }

    private static FileAttributes? Attributes(string path)
    {
        try { return File.GetAttributes(path); }
        catch (FileNotFoundException) { return null; }
        catch (DirectoryNotFoundException) { return null; }
    }

    private static void AssertNoLinks(string fullPath)
    {
        // Includes the root and its ancestors. Do not follow symlinks/junctions outside the root.
        var ancestors = new Stack<string>();
        for (var current = fullPath; current is not null; current = Path.GetDirectoryName(current))
        {
            ancestors.Push(current);
        }
        foreach (var current in ancestors)
        {
            if (Attributes(current)?.HasFlag(FileAttributes.ReparsePoint) == true)
            {
                throw new IOException("Storage paths must not contain symbolic links or reparse points.");
            }
        }
    }

    private static void EnsureDirectory(string path)
    {
        AssertNoLinks(path);
        Directory.CreateDirectory(path);
        AssertNoLinks(path);
    }
}
