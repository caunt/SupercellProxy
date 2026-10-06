namespace SupercellProxy.Networking.Assets.Retention;

/// <summary>Records an original relative asset path and the checksum of its retained bytes.</summary>
public sealed record RetainedAssetFile(string File, long Length, string Sha256, string SourceSha);
