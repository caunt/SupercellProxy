namespace SupercellProxy.Networking.Assets;

internal sealed class GameAssetCache(HttpClient webClient, Func<string?> assetDirectory)
{
    private const int MaximumConcurrentDownloads = 4;

    internal async Task<GameAsset[]> GetAssetsAsync(GameAssetFingerprint fingerprint, IEnumerable<string> downloadUrls, CancellationToken cancellationToken = default)
    {
        DirectoryInfo assetsDirectory = Directory.CreateDirectory(Path.Combine(assetDirectory() ?? GameAsset.RootDirectoryPath, fingerprint.Version, fingerprint.Sha));

        GameAsset?[] resources = new GameAsset?[fingerprint.Files.Count];
        bool[] downloadedAssets = new bool[fingerprint.Files.Count];
        string[] addresses = [.. downloadUrls];

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

        if (downloadedAssetCount > 0)
            Console.WriteLine($"Downloaded {downloadedAssetCount} game assets.");

        return [.. resources.OfType<GameAsset>()];
    }

    private static async Task<GameAsset> ReadAssetAsync(GameAssetFingerprintEntry file, string filePath, CancellationToken cancellationToken)
    {
        return new GameAsset(file, await File.ReadAllBytesAsync(filePath, cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
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
