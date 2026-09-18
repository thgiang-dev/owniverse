namespace Owniverse.Infrastructure.Storage;

public sealed class AssetStorageOptions
{
    public string Provider { get; set; } = "local";
    public string RootPath { get; set; } = string.Empty;

    internal static string ResolveRoot(string? path, string contentRootPath)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return string.Empty;
        }
        try
        {
            if (Path.IsPathRooted(path) && !Path.IsPathFullyQualified(path))
            {
                return string.Empty;
            }
            var fullPath = Path.TrimEndingDirectorySeparator(Path.GetFullPath(path, contentRootPath));
            var output = Path.TrimEndingDirectorySeparator(Path.GetFullPath(AppContext.BaseDirectory));
            var comparison = OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            if (fullPath == Path.GetPathRoot(fullPath)
                || fullPath.Equals(output, comparison)
                || fullPath.StartsWith(output + Path.DirectorySeparatorChar, comparison)
                || fullPath.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            {
                return string.Empty;
            }
            var segments = fullPath[Path.GetPathRoot(fullPath)!.Length..].Split(Path.DirectorySeparatorChar);
            if (segments.Any(segment => segment.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0
                || (OperatingSystem.IsWindows() && (segment.EndsWith('.') || segment.EndsWith(' ')))))
            {
                return string.Empty;
            }
            return fullPath;
        }
        catch (Exception exception) when (exception is ArgumentException or NotSupportedException or IOException)
        {
            return string.Empty;
        }
    }
}
