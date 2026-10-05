using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events;

/// <summary>Identifies a downloadable event asset by its server path and content fingerprint.</summary>
public sealed record EventAssetDefinition
{
    /// <summary>Gets the asset content's SHA-1 fingerprint.</summary>
    [JsonPropertyName("ChecksumSha1")]
    public string? ContentSha1 { get; init; }

    /// <summary>Gets the server-provided asset path.</summary>
    [JsonPropertyName("Path")]
    public string? Path { get; init; }
}
