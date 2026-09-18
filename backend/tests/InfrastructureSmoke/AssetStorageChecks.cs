using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Owniverse.Infrastructure.Storage;
using Owniverse.Shared.Storage;

internal static class AssetStorageChecks
{
    public static async Task<int> RunAsync()
    {
        var temporary = Directory.CreateTempSubdirectory("owniverse-assets-").FullName;
        var root = Path.Combine(temporary, "objects");
        var outside = Path.Combine(temporary, "outside");
        var link = Path.Combine(root, "linked");
        try
        {
            await using var provider = CreateProvider(root, temporary);
            var storage = provider.GetRequiredService<IAssetStorage>();
            var bytes = new byte[1024 * 1024 + 17];
            new Random(42).NextBytes(bytes);
            using (var source = new ChunkedStream(bytes))
            {
                await storage.UploadAsync("nested/images/asset.bin", source);
                Check(source.CanRead, "Caller input remains open");
            }
            await using (var actual = await storage.OpenReadAsync("nested/images/asset.bin"))
            {
                using var copy = new MemoryStream();
                await actual.CopyToAsync(copy);
                Check(copy.ToArray().SequenceEqual(bytes), "Streamed binary integrity");
            }
            Check(await storage.ExistsAsync("nested/images/asset.bin"), "Nested key exists");
            await ThrowsAsync<IOException>(() => storage.UploadAsync("nested/images/asset.bin", new MemoryStream([0])));
            await using (var unchanged = await storage.OpenReadAsync("nested/images/asset.bin"))
            {
                Check(unchanged.Length == bytes.Length && unchanged.ReadByte() == bytes[0], "Duplicate upload preserves object");
            }
            Check(!await storage.ExistsAsync("missing/asset.bin"), "Missing object returns false");
            await ThrowsAsync<FileNotFoundException>(() => storage.OpenReadAsync("missing/asset.bin"));
            await storage.DeleteAsync("missing/asset.bin");
            await storage.DeleteAsync("nested/images/asset.bin");
            await storage.DeleteAsync("nested/images/asset.bin");
            Check(!await storage.ExistsAsync("nested/images/asset.bin"), "Delete and repeated delete");

            var invalidKeys = new[] { "", "../escape", "../../secret.txt", "a/../escape", ".", "..",
                "/absolute", @"..\escape", @"C:\escape", @"C:escape", @"\\server\share", "a//b",
                "a/", "file:stream", "file.", "file ", "CON", "aux.txt", "com1.bin", "lpt9.bin",
                "%2e%2e/escape", "a/%2f/escape", "a/./b", "a\0b", ".upload-hidden.tmp" };
            foreach (var key in invalidKeys)
            {
                await ThrowsAsync<ArgumentException>(() => storage.UploadAsync(key, new MemoryStream([1])));
                await ThrowsAsync<ArgumentException>(() => storage.OpenReadAsync(key));
                await ThrowsAsync<ArgumentException>(() => storage.ExistsAsync(key));
                await ThrowsAsync<ArgumentException>(() => storage.DeleteAsync(key));
            }
            Console.WriteLine($"PASS: {invalidKeys.Length} unsafe keys rejected by all four operations");

            using (var failing = new ChunkedStream(bytes, failAfterFirstRead: true))
            {
                await ThrowsAsync<IOException>(() => storage.UploadAsync("failed.bin", failing));
            }
            Check(!await storage.ExistsAsync("failed.bin"), "Failed upload is not visible");
            using (var cancellation = new CancellationTokenSource())
            using (var cancelling = new ChunkedStream(bytes, cancelAfterFirstRead: cancellation))
            {
                await ThrowsAsync<OperationCanceledException>(() => storage.UploadAsync("cancelled.bin", cancelling, cancellation.Token));
            }
            Check(!await storage.ExistsAsync("cancelled.bin"), "Cancelled upload is not visible");
            Check(!Directory.EnumerateFiles(root, ".upload-*", SearchOption.AllDirectories).Any(), "Upload staging files cleaned");

            async Task<bool> Compete(byte value)
            {
                try { await storage.UploadAsync("race.bin", new MemoryStream([value])); return true; }
                catch (IOException) { return false; }
            }
            var outcomes = await Task.WhenAll(Compete(1), Compete(2));
            Check(outcomes.Count(success => success) == 1, "Concurrent writes have exactly one winner");
            await using (var winner = await storage.OpenReadAsync("race.bin"))
            {
                Check(winner.Length == 1 && winner.ReadByte() == (outcomes[0] ? 1 : 2), "Winning bytes preserved");
            }
            await storage.DeleteAsync("race.bin");

            Check(await Health(provider) == HealthStatus.Healthy, "Storage readiness healthy");
            Check(!Directory.EnumerateFiles(root, "*", SearchOption.AllDirectories).Any(), "Health probe cleaned up");

            Directory.CreateDirectory(outside);
            var sentinel = Path.Combine(outside, "sentinel.bin");
            await File.WriteAllBytesAsync(sentinel, bytes);
            CreateDirectoryLink(link, outside);
            await ThrowsAsync<IOException>(() => storage.UploadAsync("linked/new.bin", new MemoryStream([1])));
            await ThrowsAsync<IOException>(() => storage.OpenReadAsync("linked/sentinel.bin"));
            await ThrowsAsync<IOException>(() => storage.ExistsAsync("linked/sentinel.bin"));
            await ThrowsAsync<IOException>(() => storage.DeleteAsync("linked/sentinel.bin"));
            await using (var linkedRoot = CreateProvider(link, temporary))
            {
                Check(await Health(linkedRoot) == HealthStatus.Unhealthy, "Linked storage root is rejected");
            }
            Check((await File.ReadAllBytesAsync(sentinel)).SequenceEqual(bytes)
                && !File.Exists(Path.Combine(outside, "new.bin")), "No access escaped through junction/symlink");
            Directory.Delete(link); // Remove the link itself, never recurse through it.

            var blocker = Path.Combine(temporary, "not-a-directory");
            await File.WriteAllTextAsync(blocker, "keep");
            await using (var unavailable = CreateProvider(blocker, temporary))
            {
                Check(await Health(unavailable) == HealthStatus.Unhealthy, "Unusable storage root is unhealthy");
            }
            Check(await File.ReadAllTextAsync(blocker) == "keep", "Readiness failure preserves existing files");
            foreach (var invalid in new[] { "", "   ", "bad\0path", Path.GetPathRoot(temporary)!, AppContext.BaseDirectory })
            {
                await using var invalidProvider = CreateProvider(invalid, temporary);
                await ThrowsAsync<OptionsValidationException>(() => Task.FromResult(invalidProvider.GetRequiredService<IAssetStorage>()));
            }
            if (OperatingSystem.IsWindows())
            {
                await using var invalidPath = CreateProvider("bad*path", temporary);
                await ThrowsAsync<OptionsValidationException>(() => Task.FromResult(invalidPath.GetRequiredService<IAssetStorage>()));
            }
            await using (var unsupported = CreateProvider(root, temporary, "s3"))
            {
                await ThrowsAsync<OptionsValidationException>(() => Task.FromResult(unsupported.GetRequiredService<IAssetStorage>()));
            }
            await using (var relative = CreateProvider("relative-assets", temporary))
            {
                Check(relative.GetRequiredService<IOptions<AssetStorageOptions>>().Value.RootPath
                    == Path.Combine(temporary, "relative-assets"), "Relative root anchored to content root");
            }
            Console.WriteLine("PASS: storage configuration validation");
        }
        finally
        {
            var full = Path.GetFullPath(temporary);
            var prefix = Path.TrimEndingDirectorySeparator(Path.GetFullPath(Path.GetTempPath())) + Path.DirectorySeparatorChar;
            if (!full.StartsWith(prefix, OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal)
                || !Path.GetFileName(full).StartsWith("owniverse-assets-", StringComparison.Ordinal))
            {
                throw new InvalidOperationException("Refusing cleanup outside the owned test directory.");
            }
            if (Directory.Exists(link) && File.GetAttributes(link).HasFlag(FileAttributes.ReparsePoint))
            {
                Directory.Delete(link);
            }
            Directory.Delete(full, recursive: true);
            Check(!Directory.Exists(full), "Temporary test assets cleaned up");
        }
        return 0;
    }

    private static ServiceProvider CreateProvider(string path, string contentRoot, string provider = "local")
    {
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["ASSET_STORAGE_PROVIDER"] = provider,
            ["ASSET_STORAGE_PATH"] = path
        }).Build();
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAssetStorage(configuration, contentRoot);
        return services.BuildServiceProvider();
    }

    private static async Task<HealthStatus> Health(ServiceProvider provider) =>
        (await provider.GetRequiredService<HealthCheckService>().CheckHealthAsync()).Status;

    private static void Check(bool passed, string description)
    {
        if (!passed) { throw new InvalidOperationException(description); }
        Console.WriteLine($"PASS: {description}");
    }

    private static async Task ThrowsAsync<T>(Func<Task> action) where T : Exception
    {
        try { await action(); }
        catch (T) { return; }
        throw new InvalidOperationException($"Expected {typeof(T).Name}.");
    }

    private static void CreateDirectoryLink(string link, string target)
    {
        if (!OperatingSystem.IsWindows())
        {
            Directory.CreateSymbolicLink(link, target);
            return;
        }
        var start = new ProcessStartInfo("cmd.exe") { UseShellExecute = false, CreateNoWindow = true,
            RedirectStandardOutput = true, RedirectStandardError = true };
        foreach (var argument in new[] { "/c", "mklink", "/J", link, target }) { start.ArgumentList.Add(argument); }
        using var process = Process.Start(start)!;
        if (!process.WaitForExit(10000)) { process.Kill(); throw new IOException("Test junction creation timed out."); }
        if (process.ExitCode != 0) { throw new IOException("Test junction creation failed."); }
    }

    private sealed class ChunkedStream(byte[] bytes, bool failAfterFirstRead = false,
        CancellationTokenSource? cancelAfterFirstRead = null) : Stream
    {
        private int position;
        private bool disposed;
        public override bool CanRead => !disposed;
        protected override void Dispose(bool disposing)
        {
            disposed = true;
            base.Dispose(disposing);
        }
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => throw new NotSupportedException();
        public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }
        public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();
        public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (failAfterFirstRead && position > 0) { throw new IOException("Injected stream failure."); }
            var count = Math.Min(Math.Min(buffer.Length, 4096), bytes.Length - position);
            bytes.AsMemory(position, count).CopyTo(buffer);
            position += count;
            cancelAfterFirstRead?.Cancel();
            return ValueTask.FromResult(count);
        }
        public override void Flush() => throw new NotSupportedException();
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
    }
}
