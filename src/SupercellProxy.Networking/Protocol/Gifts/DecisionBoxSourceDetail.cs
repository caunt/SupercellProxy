using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Gifts;

/// <summary>One saved source attribute attached to a pending decision box.</summary>
public sealed record DecisionBoxSourceDetail
{
    /// <summary>Gets the attribute name.</summary>
    [JsonPropertyName("key")]
    public string Key { get; init; } = string.Empty;

    /// <summary>Gets the attribute value.</summary>
    [JsonPropertyName("value")]
    public string Value { get; init; } = string.Empty;
}
