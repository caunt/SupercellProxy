using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events;

/// <summary>Identifies a downloadable event asset by its server path and content fingerprint.</summary>
public sealed record EventAssetDefinition
{
    /// <summary>Gets or sets the asset content's SHA-1 fingerprint.</summary>
    /// <value>The <c>ChecksumSha1</c> string the event gives for the file's content, used to tell whether a downloaded copy is current, or <see langword="null"/> when the event omits it.</value>
    [JsonPropertyName("ChecksumSha1")]
    public string? ContentSha1 { get; init; }

    /// <summary>Gets or sets the server-provided asset path.</summary>
    /// <value>The <c>Path</c> under which the server serves the file, kept exactly as sent, or <see langword="null"/> when the event omits it.</value>
    [JsonPropertyName("Path")]
    public string? Path { get; init; }
}
