using SupercellProxy.Networking.Assets.Tables;


namespace SupercellProxy.Networking.Assets;

/// <summary>Loads a local fingerprint directory for asset-dependent protocol codecs.</summary>
public static class GameAssetDirectory
{
    /// <summary>The original server catalog retained alongside downloaded asset files.</summary>
    public const string CatalogFileName = "catalog.json";

    /// <summary>Reads asset files without constructing or executing game state.</summary>
    /// <param name="directory">Folder holding an unpacked asset bundle. Every file below it, in all subfolders, becomes an asset named
    /// by its path relative to this folder with <c>/</c> separators (for example <c>data/animals.csv</c>).</param>
    /// <param name="cancellationToken">Checked before each file and passed to every read; cancelling stops the load with an
    /// <see cref="OperationCanceledException"/>.</param>
    /// <returns>A new, unshared resolver over all files found, read in ordinal path order; the assets carry an empty content hash
    /// because no server manifest is involved.</returns>
    /// <exception cref="ArgumentException"><paramref name="directory"/> is <see langword="null"/>, empty or only white space.</exception>
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
            assets.Add(new GameAsset(new GameAssetFingerprintEntry(Path.GetRelativePath(directory, file).Replace(Path.DirectorySeparatorChar, newChar: '/'), string.Empty),
                await File.ReadAllBytesAsync(file, cancellationToken).ConfigureAwait(continueOnCapturedContext: false)));
        }

        return new DataTableResolver(assets);
    }
}
