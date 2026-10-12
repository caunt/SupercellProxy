using System.Text.Json;

namespace SupercellProxy.Networking.Assets;

internal sealed class GameAssetCache(HttpClient webClient, Func<string?> assetDirectory)
{
    private const int MaximumConcurrentDownloads = 4;

    // Asset files are immutable per fingerprint, so every connection in the process shares one loaded copy instead of reading them again at each login.
    private static readonly System.Collections.Concurrent.ConcurrentDictionary<string, Task<GameAsset[]>> Loaded = new(StringComparer.Ordinal);

    internal async Task<GameAsset[]> GetAssetsAsync(GameAssetFingerprint fingerprint, IEnumerable<string> downloadUrls, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(fingerprint);

        string directory = Path.Combine(assetDirectory() ?? UserDataPaths.AssetDirectoryPath, fingerprint.Version, fingerprint.Sha);
        string[] addresses = [.. downloadUrls];
        Task<GameAsset[]> loading = Loaded.GetOrAdd(directory, path => LoadAssetsAsync(path, fingerprint, addresses));

        try
        {
            return await loading.WaitAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        }
        finally
        {
            // A failed load is forgotten so the next login retries it instead of reusing the failure.
            if (loading.IsFaulted) Forget(directory, loading);
        }
    }

    private static void Forget(string directory, Task<GameAsset[]> loading)
    {
        bool removed = Loaded.TryRemove(new KeyValuePair<string, Task<GameAsset[]>>(directory, loading));

        if (!removed) return;
    }

    private static async Task<GameAsset> ReadAssetAsync(GameAssetFingerprintEntry file, string filePath, CancellationToken cancellationToken)
    {
        return new GameAsset(file, await File.ReadAllBytesAsync(filePath, cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
    }

    private static async Task SaveCatalogAsync(string directory, GameAssetFingerprint fingerprint, CancellationToken cancellationToken)
    {
        string path = Path.Combine(directory, GameAssetDirectory.CatalogFileName);
        string temporary = $"{path}.{Guid.NewGuid():N}.pending";

        try
        {
            await File.WriteAllBytesAsync(temporary, JsonSerializer.SerializeToUtf8Bytes(fingerprint), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
            File.Move(temporary, path, overwrite: true);
        }
        finally
        {
            if (File.Exists(temporary)) File.Delete(temporary);
        }
    }

    private async Task<GameAsset?> GetAssetAsync(
        GameAssetFingerprint fingerprint,
        GameAssetFingerprintEntry file,
        string filePath,
        IEnumerable<string> downloadUrls,
        CancellationToken cancellationToken
    )
    {
        if (File.Exists(filePath))
            return await ReadAssetAsync(file, filePath, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        if (Path.GetDirectoryName(filePath) is { } directoryName)
            new DirectoryInfo(directoryName).Create();

        foreach (string downloadAddress in downloadUrls)
        {
            bool downloaded = await TryDownloadAssetAsync(fingerprint, file, filePath, downloadAddress, cancellationToken)
                    .ConfigureAwait(continueOnCapturedContext: false);

            if (!downloaded)
                continue;

            return await ReadAssetAsync(file, filePath, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        }

        return null;
    }

    private async Task<GameAsset[]> LoadAssetsAsync(string directory, GameAssetFingerprint fingerprint, string[] addresses)
    {
        CancellationToken cancellationToken = CancellationToken.None;
        DirectoryInfo assetsDirectory = Directory.CreateDirectory(directory);

        GameAsset?[] resources = new GameAsset?[fingerprint.Files.Count];
        bool[] downloadedAssets = new bool[fingerprint.Files.Count];

        await Parallel
            .ForAsync(
                fromInclusive: 0,
                fingerprint.Files.Count,
                new ParallelOptions
                {
                    MaxDegreeOfParallelism = MaximumConcurrentDownloads,
                    CancellationToken = cancellationToken,
                },
                async (index, token) =>
                {
                    GameAssetFingerprintEntry file = fingerprint.Files[index];
                    string filePath = Path.Combine(assetsDirectory.FullName, file.File);
                    bool alreadyCached = File.Exists(filePath);

                    GameAsset? resource = await GetAssetAsync(fingerprint, file, filePath, addresses, token)
                        .ConfigureAwait(continueOnCapturedContext: false);

                    resources[index] = resource;
                    downloadedAssets[index] = !alreadyCached && resource is not null;
                }
            )
            .ConfigureAwait(continueOnCapturedContext: false);

        int downloadedAssetCount = downloadedAssets.Count(static downloaded => downloaded);

        if (resources.Any(static resource => resource is null))
            throw new IOException($"The asset catalog {fingerprint.Sha} could not be downloaded completely.");

        await SaveCatalogAsync(assetsDirectory.FullName, fingerprint, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        if (downloadedAssetCount > 0)
            Console.WriteLine($"Downloaded {downloadedAssetCount} game assets.");

        return [.. resources.OfType<GameAsset>()];
    }

    private async Task<bool> TryDownloadAssetAsync(
        GameAssetFingerprint fingerprint,
        GameAssetFingerprintEntry file,
        string filePath,
        string downloadAddress,
        CancellationToken cancellationToken
    )
    {
        try
        {
            Uri assetAddress = new($"{downloadAddress.Trim(trimChar: '/')}/{fingerprint.Sha.Trim(trimChar: '/')}/{file.File.Trim(trimChar: '/')}");

            using HttpResponseMessage response = await webClient
                .GetAsync(assetAddress, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            FileStream fileStream = File.Create(filePath);

            await using (fileStream.ConfigureAwait(continueOnCapturedContext: false))
            {
                await response.EnsureSuccessStatusCode()
                    .Content.CopyToAsync(fileStream, cancellationToken)
                    .ConfigureAwait(continueOnCapturedContext: false);
            }

            return true;
        }
        catch (OperationCanceledException)
        {
            File.Delete(filePath);

            throw;
        }
        catch (Exception exception)
            when (exception is HttpRequestException or IOException)
        {
            File.Delete(filePath);
            Console.WriteLine($"Failed to download {file.File} from {downloadAddress}: {exception.Message}");

            return false;
        }
    }
}
