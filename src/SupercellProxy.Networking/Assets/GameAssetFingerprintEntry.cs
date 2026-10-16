namespace SupercellProxy.Networking.Assets;

/// <summary>
/// One file listed in the asset manifest.
/// </summary>
/// <param name="File">Path of the file inside the bundle, such as <c>data/animals.csv</c>.</param>
/// <param name="Sha">Content hash the manifest lists for this file; it changes whenever the file's content changes.</param>
public sealed record GameAssetFingerprintEntry(string File, string Sha);
