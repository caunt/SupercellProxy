using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>A set of rewards a Farm Pass level gives together.</summary>
public sealed record FarmPassRewardGroupDefinition
{
    /// <summary>What the set gives, each named by its table entry with an amount.</summary>
    [JsonPropertyName("rewards")]
    public FarmPassRewardDefinition[] Rewards { get; init; } = [];
}
