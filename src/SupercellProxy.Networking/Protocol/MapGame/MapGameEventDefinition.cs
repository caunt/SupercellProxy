using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.MapGame.Rewards;

namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>Live event describing one Valley season: its map, its exclusive shop rewards and the goal that unlocks them.</summary>
public sealed record MapGameEventDefinition
{
    /// <summary>Gets or sets the collection goal that unlocks the exclusive rewards.</summary>
    /// <value>The goal read from <c>tier2RewardsGoalType</c>; <see langword="null"/> when the definition names none.</value>
    [JsonPropertyName("tier2RewardsGoalType")]
    public MapGameCollectionGoalDefinition? CollectionGoal { get; init; }

    /// <summary>Gets or sets the ordered alternatives for the first exclusive reward.</summary>
    /// <value>Candidates for the first slot, in definition order; empty by default.</value>
    [JsonPropertyName("shopExclusiveRewardSlot1")]
    public MapGameExclusiveRewardDefinition[] ExclusiveRewardSlot1 { get; init; } = [];

    /// <summary>Gets or sets the ordered alternatives for the second exclusive reward.</summary>
    /// <value>Candidates for the second slot, in definition order like <see cref="ExclusiveRewardSlot1"/>; empty by default.</value>
    [JsonPropertyName("shopExclusiveRewardSlot2")]
    public MapGameExclusiveRewardDefinition[] ExclusiveRewardSlot2 { get; init; } = [];

    /// <summary>Gets or sets the ordered alternatives for the third exclusive reward.</summary>
    /// <value>Candidates for the third slot, in definition order like <see cref="ExclusiveRewardSlot1"/>; empty by default.</value>
    [JsonPropertyName("shopExclusiveRewardSlot3")]
    public MapGameExclusiveRewardDefinition[] ExclusiveRewardSlot3 { get; init; } = [];

    /// <summary>Gets or sets the map selected by this season.</summary>
    /// <value>The map's name as written in <c>mapGameMap</c>; <see langword="null"/> when the definition names none.</value>
    [JsonPropertyName("mapGameMap")]
    public string? MapName { get; init; }

    /// <summary>Gets or sets whether the season is on hold.</summary>
    /// <value><see langword="true"/> if the event's <c>pause</c> flag is set; otherwise, <see langword="false"/>.</value>
    /// <remarks>The flag defaults to <see langword="false"/>.</remarks>
    [JsonPropertyName("pause")]
    public bool Pause { get; init; }
}
