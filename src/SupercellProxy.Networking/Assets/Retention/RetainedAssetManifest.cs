namespace SupercellProxy.Networking.Assets.Retention;

/// <summary>Describes one complete immutable asset snapshot shared by recordings.</summary>
/// <param name="Fingerprint">Bundle hash the server published for the snapshot's asset set; loading refuses a manifest whose
/// fingerprint differs from the one requested.</param>
/// <param name="Files">Every retained file, sorted by path; a manifest without files, or with a repeated path, is refused on
/// load.</param>
public sealed record RetainedAssetManifest(string Fingerprint, RetainedAssetFile[] Files)
{
    /// <summary>The snapshot document, separate from the game's own asset files.</summary>
    public const string FileName = "assets.json";
}
