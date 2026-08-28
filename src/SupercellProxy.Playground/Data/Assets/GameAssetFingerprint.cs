namespace SupercellProxy.Playground.Data.Assets;

/// <summary>
/// Represents <c language="csharp">GameAssetFingerprint</c>.
/// </summary>
internal sealed record GameAssetFingerprint(
    IReadOnlyList<GameAssetFingerprintEntry> Files,
    string Sha,
    string Version
);
