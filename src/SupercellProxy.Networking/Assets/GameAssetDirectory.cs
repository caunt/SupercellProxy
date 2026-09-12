using SupercellProxy.Networking.Assets.Tables;


namespace SupercellProxy.Networking.Assets;

/// <summary>Loads a local fingerprint directory for asset-dependent protocol codecs.</summary>
public static class GameAssetDirectory
{
    /// <summary>Reads asset files without constructing or executing game state.</summary>
    public static async Task<DataTableResolver> LoadAsync(string directory, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(directory);

        IOrderedEnumerable<string> files = Directory
            .EnumerateFiles(directory, searchPattern: "*", SearchOption.AllDirectories)
            .Order(StringComparer.Ordinal);

        List<GameAsset> assets = [];

        foreach (string file in files)
        {
            cancellationToken.ThrowIfCancellationRequested();
            assets.Add(
                new GameAsset(
                    new GameAssetFingerprintEntry(Path.GetRelativePath(directory, file).Replace(Path.DirectorySeparatorChar, newChar: '/'), string.Empty),
                    await File.ReadAllBytesAsync(file, cancellationToken).ConfigureAwait(continueOnCapturedContext: false)
                )
            );
        }

        return new DataTableResolver(assets);
    }
}
