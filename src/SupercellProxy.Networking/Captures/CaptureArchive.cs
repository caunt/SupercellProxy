using System.Collections.Concurrent;

using Nito.AsyncEx;

using SupercellProxy.Networking.Assets.Retention;

namespace SupercellProxy.Networking.Captures;

/// <summary>Groups recordings by client release and retains their fingerprint-specific assets.</summary>
public sealed class CaptureArchive(string root, string? assetCache = null)
{
    /// <summary>The directory shared by all asset snapshots for one recorded game release.</summary>
    public const string AssetsDirectoryName = "assets";
    private readonly ConcurrentDictionary<CaptureIdentity, AsyncLock> _gates = [];
    private readonly ConcurrentDictionary<CaptureIdentity, string> _retained = [];

    /// <summary>Ensures assets are independently retained before publishing a recording.</summary>
    public async Task<string> RetainAssetsAsync(CaptureIdentity identity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identity);
        identity.Validate();

        bool available = _retained.TryGetValue(identity, out string? retained)
            && File.Exists(Path.Combine(retained, RetainedAssetManifest.FileName));

        if (available) return retained ?? throw new InvalidDataException(message: "The retained asset path is missing.");

        using IDisposable lease = await _gates.GetOrAdd(identity, static unused => new AsyncLock()).LockAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        available = _retained.TryGetValue(identity, out retained) && File.Exists(Path.Combine(retained, RetainedAssetManifest.FileName));

        if (available) return retained ?? throw new InvalidDataException(message: "The retained asset path is missing.");

        string destination = Path.Combine(VersionDirectory(identity), AssetsDirectoryName, identity.AssetFingerprint);
        string source = Directory.Exists(destination) ? destination : GameAssetSnapshot.FindSource(assetCache ?? UserDataPaths.AssetDirectoryPath, identity.AssetFingerprint);
        await GameAssetSnapshot.RetainAsync(source, destination, identity.AssetFingerprint, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        _retained[identity] = destination;

        return destination;
    }

    /// <summary>Gets the recording directory for a game release.</summary>
    public string VersionDirectory(CaptureIdentity identity)
    {
        ArgumentNullException.ThrowIfNull(identity);
        identity.Validate();

        return Path.GetFullPath(Path.Combine(root, identity.GameVersion));
    }
}
