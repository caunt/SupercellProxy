using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Rewards;

/// <summary>Defines one event-configured exclusive Valley reward alternative.</summary>
public sealed record MapGameExclusiveRewardDefinition
{
    /// <summary>Gets the resources included in the reward.</summary>
    [JsonPropertyName("rewards")]
    public MapGameRewardResourceDefinition[] Rewards { get; init; } = [];

    /// <summary>Gets the token prices in native order.</summary>
    [JsonPropertyName("rewardPrices")]
    public MapGameRewardPriceDefinition[] Prices { get; init; } = [];

    /// <summary>Gets the optional localized reward title.</summary>
    [JsonPropertyName("rewardTitle")]
    public string? Title { get; init; }
}
