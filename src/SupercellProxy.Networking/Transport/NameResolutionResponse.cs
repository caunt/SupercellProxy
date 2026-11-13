using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Transport;

/// <summary>Contains address answers returned by the DNS-over-HTTPS endpoint.</summary>
public sealed record NameResolutionResponse
{
    /// <summary>Gets or sets the returned address answers.</summary>
    /// <value>The JSON <c>Answer</c> array in the order the DNS-over-HTTPS server returned it; the resolver takes the first entry that parses as an IPv4 address.</value>
    [JsonPropertyName("Answer")]
    public required ImmutableArray<NameResolutionAnswer> Answers { get; init; }
}
