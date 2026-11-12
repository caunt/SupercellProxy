using System.Security.Cryptography;
using System.Text.Json;

namespace SupercellProxy.Networking.Assets.Retention;

/// <summary>Retains independent copies of game assets and verifies them before replay.</summary>
public static class GameAssetSnapshot
{
    /// <summary>Finds a cached fingerprint without selecting assets by release ordering or modification time.</summary>
    /// <param name="root">Asset cache folder to search, such as the per-version download folder; it is searched itself and in every
    /// subfolder.</param>
    /// <param name="fingerprint">Bundle hash the server published (the catalog's <see cref="GameAssetFingerprint.Sha"/>), matched
    /// against folder names without regard to letter case.</param>
    /// <returns>The shortest matching folder path (ties broken by ordinal order) that is named after <paramref name="fingerprint"/>
    /// and contains <c>data/production_buildings_goods.csv</c>.</returns>
    /// <exception cref="DirectoryNotFoundException"><paramref name="root"/> does not exist, or no folder below it is named after
    /// <paramref name="fingerprint"/> and holds the production goods table.</exception>
    public static string FindSource(string root, string fingerprint)
    {
        if (!Directory.Exists(root))
            throw new DirectoryNotFoundException($"The asset cache {root} is unavailable.");

        IEnumerable<string> candidates = Directory.EnumerateDirectories(root, fingerprint, SearchOption.AllDirectories).Prepend(root);

        return candidates.Where(path => string.Equals(Path.GetFileName(path), fingerprint, StringComparison.OrdinalIgnoreCase))
            .Where(path => File.Exists(Path.Combine(path, GameAssetFiles.ProductionBuildingsGoods)))
            .OrderBy(static path => path.Length).ThenBy(static path => path, StringComparer.Ordinal).FirstOrDefault()
            ?? throw new DirectoryNotFoundException($"Assets for fingerprint {fingerprint} are unavailable in {root}.");
    }

    /// <summary>Loads only the manifest's files, requiring their original retained checksums.</summary>
    /// <param name="directory">Retained snapshot folder that holds <c>assets.json</c> next to the asset files it lists.</param>
    /// <param name="fingerprint">Bundle hash the snapshot must have been retained for; it is compared to the manifest's
    /// <see cref="RetainedAssetManifest.Fingerprint"/> without regard to letter case.</param>
    /// <param name="cancellationToken">Passed to every file read; cancelling stops the load with an
    /// <see cref="OperationCanceledException"/>.</param>
    /// <returns>One asset per manifest entry, in manifest order, each named with its original path and the server hash it was
    /// downloaded under.</returns>
    /// <exception cref="FileNotFoundException"><paramref name="directory"/> has no <c>assets.json</c> manifest.</exception>
    /// <exception cref="InvalidDataException">The manifest is empty, names another fingerprint or no files, repeats a path, lists a path
    /// outside <paramref name="directory"/>, or has no production goods table, or a file's length or SHA-256 no longer matches the
    /// manifest.</exception>
    public static async Task<GameAsset[]> LoadAsync(string directory, string fingerprint, CancellationToken cancellationToken = default)
    {
        string manifestPath = Path.Combine(directory, RetainedAssetManifest.FileName);

        if (!File.Exists(manifestPath))
            throw new FileNotFoundException($"Retained assets {fingerprint} are unavailable.", manifestPath);

        RetainedAssetManifest manifest = JsonSerializer.Deserialize<RetainedAssetManifest>(await File.ReadAllBytesAsync(manifestPath, cancellationToken).ConfigureAwait(continueOnCapturedContext: false))
            ?? throw new InvalidDataException(message: "The retained asset manifest is empty.");

        bool valid = string.Equals(manifest.Fingerprint, fingerprint, StringComparison.OrdinalIgnoreCase) && manifest.Files is { Length: > 0 };

        if (!valid)
            throw new InvalidDataException(message: "The retained asset manifest has another fingerprint or no files.");

        HashSet<string> names = new(StringComparer.Ordinal);
        GameAsset[] assets = new GameAsset[manifest.Files.Length];

        for (int index = 0; index < assets.Length; index++)
        {
            RetainedAssetFile file = manifest.Files[index];

            if (!names.Add(file.File))
                throw new InvalidDataException($"The retained asset manifest repeats {file.File}.");

            byte[] bytes = await File.ReadAllBytesAsync(ResolveFile(directory, file.File), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
            bool matches = bytes.LongLength == file.Length && string.Equals(Convert.ToHexString(SHA256.HashData(bytes)), file.Sha256, StringComparison.OrdinalIgnoreCase);

            if (!matches)
                throw new InvalidDataException($"Retained asset {file.File} does not match its checksum for {fingerprint}.");

            assets[index] = new GameAsset(new GameAssetFingerprintEntry(file.File, file.SourceSha), bytes);
        }

        return !names.Contains(GameAssetFiles.ProductionBuildingsGoods)
            ? throw new InvalidDataException(message: "The retained asset snapshot has no production tables.")
            : assets;
    }

    /// <summary>Loads the exact fingerprint's cached files, including pre-manifest legacy caches.</summary>
    /// <param name="source">Folder of one bundle: a retained snapshot with <c>assets.json</c>, a download cache with
    /// <c>catalog.json</c>, or a legacy cache with neither, whose files are taken from its folder tree.</param>
    /// <param name="fingerprint">Bundle hash the files must belong to; a manifest or catalog naming another hash is refused, while a
    /// legacy cache is trusted to match it.</param>
    /// <param name="cancellationToken">Passed to every file read; cancelling stops the load with an
    /// <see cref="OperationCanceledException"/>.</param>
    /// <returns>The bundle's files as assets, checksum-verified when a manifest exists; files from a catalog keep their server hash,
    /// legacy files carry an empty one.</returns>
    /// <exception cref="InvalidDataException">The catalog is empty or names another fingerprint, the file list has no production goods
    /// table, a listed path leaves <paramref name="source"/>, or a retained manifest fails the checks of <see cref="LoadAsync"/>.</exception>
    public static async Task<GameAsset[]> LoadCachedAsync(string source, string fingerprint, CancellationToken cancellationToken = default)
    {
        if (File.Exists(Path.Combine(source, RetainedAssetManifest.FileName)))
            return await LoadAsync(source, fingerprint, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        string catalogPath = Path.Combine(source, GameAssetDirectory.CatalogFileName);
        GameAssetFingerprintEntry[] entries;

        if (File.Exists(catalogPath))
        {
            GameAssetFingerprint catalog = JsonSerializer.Deserialize<GameAssetFingerprint>(await File.ReadAllBytesAsync(catalogPath, cancellationToken).ConfigureAwait(continueOnCapturedContext: false))
                ?? throw new InvalidDataException(message: "The cached asset catalog is empty.");

            if (!string.Equals(catalog.Sha, fingerprint, StringComparison.OrdinalIgnoreCase))
                throw new InvalidDataException(message: "The cached asset catalog has another fingerprint.");

            entries = [.. catalog.Files];
        }
        else
        {
            // Legacy downloads predate retained catalogs; preserve their exact named fingerprint's files.
            entries = [.. EnumerateSourceFiles(source, source).Select(file => new GameAssetFingerprintEntry(Path.GetRelativePath(source, file).Replace(Path.DirectorySeparatorChar, newChar: '/'), string.Empty))];
        }

        if (!entries.Any(static entry => entry.File == GameAssetFiles.ProductionBuildingsGoods))
            throw new InvalidDataException(message: "The asset fingerprint has no production tables.");

        List<GameAsset> assets = [];

        foreach (GameAssetFingerprintEntry entry in entries)
        {
            byte[] bytes = await File.ReadAllBytesAsync(ResolveFile(source, entry.File), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

            assets.Add(new GameAsset(entry, bytes));
        }

        return [.. assets];
    }

    /// <summary>Atomically publishes a snapshot, sharing an existing validated copy when present.</summary>
    /// <param name="source">Cache folder to copy from, in any layout <see cref="LoadCachedAsync"/> accepts; it is not read when
    /// <paramref name="destination"/> already exists.</param>
    /// <param name="destination">Snapshot folder to create; its parent is created when missing, and an existing folder is only
    /// validated, never overwritten.</param>
    /// <param name="fingerprint">Bundle hash recorded in the new manifest and required of an existing snapshot.</param>
    /// <param name="cancellationToken">Passed to every read and write; cancelling leaves no partial snapshot behind, because files are
    /// written to a temporary folder that is deleted.</param>
    /// <exception cref="InvalidDataException">An existing snapshot fails validation or is empty, the source lacks the production goods
    /// table, <paramref name="destination"/> has no parent folder, or a concurrent writer published different files for the same
    /// fingerprint.</exception>
    public static async Task RetainAsync(string source, string destination, string fingerprint, CancellationToken cancellationToken = default)
    {
        if (Directory.Exists(destination))
        {
            GameAsset[] existing = await LoadAsync(destination, fingerprint, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

            if (existing.Length == 0)
                throw new InvalidDataException(message: "The retained asset snapshot is empty.");

            return;
        }

        GameAsset[] assets = await LoadCachedAsync(source, fingerprint, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        if (!assets.Any(static asset => asset.Fingerprint.File == GameAssetFiles.ProductionBuildingsGoods))
            throw new InvalidDataException(message: "The source does not contain the game's production tables.");

        string parent = Path.GetDirectoryName(destination) ?? throw new InvalidDataException(message: "The snapshot has no parent directory.");
        DirectoryInfo parentDirectory = Directory.CreateDirectory(parent);
        string temporary = Path.Combine(parentDirectory.FullName, $".{fingerprint}-{Guid.NewGuid():N}.pending");
        DirectoryInfo temporaryDirectory = Directory.CreateDirectory(temporary);

        try
        {
            List<RetainedAssetFile> files = [];

            foreach (GameAsset asset in assets.OrderBy(static asset => asset.Fingerprint.File, StringComparer.Ordinal))
            {
                string file = ResolveFile(temporaryDirectory.FullName, asset.Fingerprint.File);
                DirectoryInfo fileDirectory = Directory.CreateDirectory(Path.GetDirectoryName(file) ?? temporaryDirectory.FullName);
                await File.WriteAllBytesAsync(Path.Combine(fileDirectory.FullName, Path.GetFileName(file)), asset.Content.ToArray(), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                files.Add(new RetainedAssetFile(asset.Fingerprint.File, asset.Content.Length, Convert.ToHexString(SHA256.HashData(asset.Content.Span)), asset.Fingerprint.Sha));
            }

            RetainedAssetManifest manifest = new(fingerprint, Files: [.. files]);
            await File.WriteAllBytesAsync(Path.Combine(temporary, RetainedAssetManifest.FileName), JsonSerializer.SerializeToUtf8Bytes(manifest), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

            try
            {
                Directory.Move(temporary, destination);
            }
            catch (IOException) when (Directory.Exists(destination))
            {
                GameAsset[] existing = await LoadAsync(destination, fingerprint, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

                bool identical = existing.Length == files.Count && existing.All(asset => files.Any(file => file.File == asset.Fingerprint.File && file.Sha256 == Convert.ToHexString(SHA256.HashData(asset.Content.Span))));

                if (!identical)
                    throw new InvalidDataException(message: "Concurrent asset snapshots differ for the same fingerprint.");
            }
        }
        finally
        {
            if (Directory.Exists(temporary))
                Directory.Delete(temporary, recursive: true);
        }
    }

    private static IEnumerable<string> EnumerateSourceFiles(string root, string directory)
    {
        foreach (string file in Directory.EnumerateFiles(directory).Order(StringComparer.Ordinal))
        {
            if (file == Path.Combine(root, GameAssetDirectory.CatalogFileName) || file == Path.Combine(root, RetainedAssetManifest.FileName))
                continue;

            yield return file;
        }

        foreach (string child in Directory.EnumerateDirectories(directory).Order(StringComparer.Ordinal))
        {
            // Some old caches contain a second, nested fingerprint directory. It is another bundle,
            // not part of the relative file tree served by this fingerprint.
            if (File.Exists(Path.Combine(child, GameAssetFiles.ProductionBuildingsGoods)))
                continue;

            foreach (string file in EnumerateSourceFiles(root, child))
                yield return file;
        }
    }

    private static string ResolveFile(string directory, string relative)
    {
        string root = Path.GetFullPath(directory) + Path.DirectorySeparatorChar.ToString();
        string path = Path.GetFullPath(Path.Combine(root, relative));

        bool outside = Path.IsPathRooted(relative) || !path.StartsWith(root, StringComparison.Ordinal) || relative.Contains(value: '\\', StringComparison.Ordinal);

        return outside
            ? throw new InvalidDataException(message: "An asset path leaves its fingerprint directory.")
            : path;
    }
}
