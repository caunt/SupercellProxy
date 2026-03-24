namespace SupercellProxy.Playground.Resources;

public sealed record ResourceFingerprint(IReadOnlyList<ResourceFingerprintFile> Files, string Sha, string Version);
