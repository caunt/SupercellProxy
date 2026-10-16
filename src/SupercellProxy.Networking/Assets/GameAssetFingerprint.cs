namespace SupercellProxy.Networking.Assets;

/// <summary>
/// The asset manifest for one content release: every file in the bundle with its hash, plus the hash that names the whole bundle.
/// The server sends it inside a login failure when the client's resources are out of date.
/// </summary>
/// <param name="Files">Each file in the bundle with its own content hash.</param>
/// <param name="Sha">
/// Hash that names the whole bundle. Clients report it at login, and it is the folder the files are downloaded from on the asset
/// server and cached under locally.
/// </param>
/// <param name="Version">Content version string the bundle was published under; the local cache keeps one folder per version.</param>
public sealed record GameAssetFingerprint(IReadOnlyList<GameAssetFingerprintEntry> Files, string Sha, string Version);
