using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Gifts;

/// <summary>One saved source attribute attached to a pending decision box.</summary>
public sealed record DecisionBoxSourceDetail
{
    /// <summary>Gets or sets the attribute name.</summary>
    /// <value>The attribute key saved as <c>key</c>, such as <c>milestone</c> or <c>track_id</c> for order-track rewards; empty by default.</value>
    [JsonPropertyName("key")]
    public string Key { get; init; } = string.Empty;

    /// <summary>Gets or sets the attribute value.</summary>
    /// <value>The attribute's value as text, such as a milestone index written with invariant digits; empty by default.</value>
    [JsonPropertyName("value")]
    public string Value { get; init; } = string.Empty;
}
