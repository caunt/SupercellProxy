namespace SupercellProxy.Networking.Assets;

/// <summary>
/// Represents <c language="csharp">GameAssetFingerprint</c>.
/// </summary>
public sealed record GameAssetFingerprint(IReadOnlyList<GameAssetFingerprintEntry> Files, string Sha, string Version);
