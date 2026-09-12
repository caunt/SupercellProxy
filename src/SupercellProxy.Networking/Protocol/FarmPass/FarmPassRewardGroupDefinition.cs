using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>Represents the decoded FarmPassRewardGroupDefinition JSON contract.</summary>
public sealed record FarmPassRewardGroupDefinition
{
    /// <summary>Gets the Rewards value.</summary>
    [JsonPropertyName("rewards")]
    public FarmPassRewardDefinition[] Rewards { get; init; } = [];

}
