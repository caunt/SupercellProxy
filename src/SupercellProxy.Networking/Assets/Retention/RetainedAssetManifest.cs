namespace SupercellProxy.Networking.Assets.Retention;

/// <summary>Describes one complete immutable asset snapshot shared by recordings.</summary>
public sealed record RetainedAssetManifest(string Fingerprint, RetainedAssetFile[] Files)
{
    /// <summary>The snapshot document, separate from the game's own asset files.</summary>
    public const string FileName = "assets.json";
}
