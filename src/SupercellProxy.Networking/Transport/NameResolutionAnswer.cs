using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Transport;

/// <summary>Describes an address answer returned by the DNS-over-HTTPS endpoint.</summary>
public sealed record NameResolutionAnswer
{
    /// <summary>Gets the returned address text.</summary>
    [JsonPropertyName("data")]
    public required string Address { get; init; }
}
