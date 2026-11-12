using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Transport;

/// <summary>Describes an address answer returned by the DNS-over-HTTPS endpoint.</summary>
public sealed record NameResolutionAnswer
{
    /// <summary>Gets or sets the returned address text.</summary>
    /// <value>The answer's <c>data</c> field: an IPv4 address in dotted form for an A record, though other record types may carry other text, which the resolver skips.</value>
    [JsonPropertyName("data")]
    public required string Address { get; init; }
}
