namespace SupercellProxy.Networking.Assets.Retention;

/// <summary>Records an original relative asset path and the checksum of its retained bytes.</summary>
/// <param name="File">Bundle-relative path with <c>/</c> separators, such as <c>data/animals.csv</c>; it must stay inside the snapshot
/// folder.</param>
/// <param name="Length">Size of the retained file in bytes; loading refuses a file of any other size.</param>
/// <param name="Sha256">SHA-256 of the retained bytes as uppercase hexadecimal, compared without regard to letter case on load.</param>
/// <param name="SourceSha">Content hash the server catalog listed for the file when it was downloaded; empty for files from a legacy
/// cache without a catalog.</param>
public sealed record RetainedAssetFile(string File, long Length, string Sha256, string SourceSha);
