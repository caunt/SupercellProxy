using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.MapGame.Rewards;

namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>Live event describing one Valley season: its map, its exclusive shop rewards and the goal that unlocks them.</summary>
public sealed record MapGameEventDefinition
{
    /// <summary>Gets the collection goal that unlocks the exclusive rewards.</summary>
    [JsonPropertyName("tier2RewardsGoalType")]
    public MapGameCollectionGoalDefinition? CollectionGoal { get; init; }

    /// <summary>Gets the ordered alternatives for the first exclusive reward.</summary>
    [JsonPropertyName("shopExclusiveRewardSlot1")]
    public MapGameExclusiveRewardDefinition[] ExclusiveRewardSlot1 { get; init; } = [];

    /// <summary>Gets the ordered alternatives for the second exclusive reward.</summary>
    [JsonPropertyName("shopExclusiveRewardSlot2")]
    public MapGameExclusiveRewardDefinition[] ExclusiveRewardSlot2 { get; init; } = [];

    /// <summary>Gets the ordered alternatives for the third exclusive reward.</summary>
    [JsonPropertyName("shopExclusiveRewardSlot3")]
    public MapGameExclusiveRewardDefinition[] ExclusiveRewardSlot3 { get; init; } = [];

    /// <summary>Gets the map selected by this season.</summary>
    [JsonPropertyName("mapGameMap")]
    public string? MapName { get; init; }

    /// <summary>Whether the season is on hold; the client logic does not read it.</summary>
    [JsonPropertyName("pause")]
    public bool Pause { get; init; }
}
