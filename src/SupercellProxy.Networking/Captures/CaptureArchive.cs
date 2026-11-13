using System.Collections.Concurrent;

using Nito.AsyncEx;

using SupercellProxy.Networking.Assets.Retention;

namespace SupercellProxy.Networking.Captures;

/// <summary>Groups recordings by client release and retains their fingerprint-specific assets.</summary>
/// <param name="root">The archive directory; each game release gets a subdirectory named after its three-part version.</param>
/// <param name="assetCache">The directory searched for downloaded asset fingerprints to copy from, or <see langword="null"/> to use the user asset directory.</param>
public sealed class CaptureArchive(string root, string? assetCache = null)
{
    /// <summary>The directory shared by all asset snapshots for one recorded game release.</summary>
    public const string AssetsDirectoryName = "assets";
    private readonly ConcurrentDictionary<CaptureIdentity, AsyncLock> _gates = [];
    private readonly ConcurrentDictionary<CaptureIdentity, string> _retained = [];

    /// <summary>Ensures assets are independently retained before publishing a recording.</summary>
    /// <param name="identity">The release and asset fingerprint whose assets the recording needs; they are copied into <c>&lt;version&gt;/assets/&lt;fingerprint&gt;</c> under the archive root.</param>
    /// <param name="cancellationToken">Cancels waiting for another retention of the same identity and the file copy; a copy cut short is not recorded as retained.</param>
    /// <returns>The directory holding the retained assets and their manifest; a directory retained earlier in this archive is returned again without copying while its manifest still exists.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="identity"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidDataException"><paramref name="identity"/> fails <see cref="CaptureIdentity.Validate"/>, or the cached path for an already retained identity is missing.</exception>
    /// <remarks>Retention of one identity runs under its own lock, so concurrent callers copy the assets once. The source is the existing destination when it is already on disk, otherwise the matching fingerprint folder in the asset cache passed to the constructor or the user asset directory.</remarks>
    public async Task<string> RetainAssetsAsync(CaptureIdentity identity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identity);

        identity.Validate();

        bool available = _retained.TryGetValue(identity, out string? retained)
            && File.Exists(Path.Combine(retained, RetainedAssetManifest.FileName));

        if (available)
            return retained ?? throw new InvalidDataException(message: "The retained asset path is missing.");

        using IDisposable lease = await _gates.GetOrAdd(identity, static unused => new AsyncLock()).LockAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        available = _retained.TryGetValue(identity, out retained) && File.Exists(Path.Combine(retained, RetainedAssetManifest.FileName));

        if (available)
            return retained ?? throw new InvalidDataException(message: "The retained asset path is missing.");

        string destination = Path.Combine(VersionDirectory(identity), AssetsDirectoryName, identity.AssetFingerprint);
        string source = Directory.Exists(destination) ? destination : GameAssetSnapshot.FindSource(assetCache ?? UserDataPaths.AssetDirectoryPath, identity.AssetFingerprint);

        await GameAssetSnapshot.RetainAsync(source, destination, identity.AssetFingerprint, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        _retained[identity] = destination;

        return destination;
    }

    /// <summary>Gets the recording directory for a game release.</summary>
    /// <param name="identity">The recording's release and asset fingerprint; only <see cref="CaptureIdentity.GameVersion"/> picks the directory, but the whole identity must pass <see cref="CaptureIdentity.Validate"/>.</param>
    /// <returns>The absolute path of the archive root joined with the three-part game version, such as <c>root/1.73.81</c>; the directory is not created or checked.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="identity"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidDataException"><paramref name="identity"/> does not have a three-part game version and a 40-character hexadecimal asset fingerprint.</exception>
    public string VersionDirectory(CaptureIdentity identity)
    {
        ArgumentNullException.ThrowIfNull(identity);

        identity.Validate();

        return Path.GetFullPath(Path.Combine(root, identity.GameVersion));
    }
}
