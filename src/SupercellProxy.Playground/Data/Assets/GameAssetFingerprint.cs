namespace SupercellProxy.Playground.Data.Assets;

/// <summary>
/// Represents <c>GameAssetFingerprint</c>.
/// </summary>
public sealed record GameAssetFingerprint(
    IReadOnlyList<GameAssetFingerprintEntry> Files,
    string Sha,
    string Version
);
