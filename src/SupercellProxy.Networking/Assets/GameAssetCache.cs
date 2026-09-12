namespace SupercellProxy.Networking.Assets;

internal sealed class GameAssetCache(HttpClient webClient, Func<string?> assetDirectory)
{

    internal async Task<GameAsset[]> GetAssetsAsync(GameAssetFingerprint fingerprint, IEnumerable<string> downloadUrls, CancellationToken cancellationToken = default)
    {
        DirectoryInfo assetsDirectory = Directory.CreateDirectory(Path.Combine(assetDirectory() ?? GameAsset.RootDirectoryPath, fingerprint.Version, fingerprint.Sha));

        List<GameAsset> resources = [];

        foreach (GameAssetFingerprintEntry file in fingerprint.Files)
        {
            string filePath = Path.Combine(assetsDirectory.FullName, file.File);

            GameAsset? resource = await GetAssetAsync(fingerprint, file, filePath, downloadUrls, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            if (resource is not null)
                resources.Add(resource);
        }

        return [.. resources];
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

            Console.WriteLine($"Downloaded {file.File} from {downloadAddress}");

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
        catch (Exception exception)
            when (exception is HttpRequestException or IOException)
        {
            Console.WriteLine($"Failed to download {file.File} from {downloadAddress}: {exception.Message}");

            return false;
        }
    }

}
