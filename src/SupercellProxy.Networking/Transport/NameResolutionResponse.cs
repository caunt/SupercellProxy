using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Transport;

/// <summary>Contains address answers returned by the DNS-over-HTTPS endpoint.</summary>
public sealed record NameResolutionResponse
{
    /// <summary>Gets the returned address answers.</summary>
    [JsonPropertyName("Answer")]
    public required NameResolutionAnswer[] Answers { get; init; }
}
