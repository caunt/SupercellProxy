using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>A set of rewards a Farm Pass level gives together.</summary>
public sealed record FarmPassRewardGroupDefinition
{
    /// <summary>Gets or sets what the set gives, each named by its table entry with an amount.</summary>
    /// <value>The event's reward entries in listed order; empty when the set gives nothing.</value>
    [JsonPropertyName("rewards")]
    public FarmPassRewardDefinition[] Rewards { get; init; } = [];
}
