using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Neighborhoods.Rewards;

namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>Live event describing a neighbourhood event: its object, task sets, task refresh costs and reward tiers.</summary>
public sealed record NeighborhoodEventDefinition
{
    /// <summary>Diamonds the player pays to add one more task beyond the event's slots.</summary>
    [JsonPropertyName("addExtraTaskDiamondCost")]
    public int ExtraTaskDiamondCost { get; init; }

    /// <summary>Gets the task group used for purchased extra tasks.</summary>
    [JsonPropertyName("extraTaskGroup")]
    public string? ExtraTaskGroup { get; init; }

    /// <summary>Gets the maximum number of extra active tasks.</summary>
    [JsonPropertyName("maxExtraTasks")]
    public int MaximumExtraTasks { get; init; }

    /// <summary>Gets the personal contribution required to claim neighborhood rewards.</summary>
    [JsonPropertyName("minContributionToClaimRewards")]
    public int MinimumContribution { get; init; }

    /// <summary>Gets the neighborhood object selected for this event.</summary>
    [JsonPropertyName("object")]
    public string? ObjectName { get; init; }

    /// <summary>Gets the diamond cost of a paid task refresh.</summary>
    [JsonPropertyName("taskRefreshDiamondCost")]
    public int TaskRefreshDiamondCost { get; init; }

    /// <summary>Gets whether the event permits advertisement-funded task refreshes.</summary>
    [JsonPropertyName("taskRefreshForAdWatch")]
    public bool TaskRefreshForAdWatch { get; init; }

    /// <summary>Task group names one task set queues, one task per group.</summary>
    [JsonPropertyName("taskSet")]
    public string[] TaskSet { get; init; } = [];

    /// <summary>How many task sets each event week grants.</summary>
    [JsonPropertyName("taskSetsPerWeek")]
    public int TaskSetsPerWeek { get; init; }

    /// <summary>How many tasks the player can have open at once.</summary>
    [JsonPropertyName("taskSlotCount")]
    public int TaskSlotCount { get; init; }

    /// <summary>Gets the configured reward entries, including their inclusive player-level ranges.</summary>
    [JsonPropertyName("tiers")]
    public NeighborhoodRewardTierDefinition[] Tiers { get; init; } = [];

    /// <summary>Gets the neighborhood object's visual milestones.</summary>
    [JsonPropertyName("visualLevels")]
    public NeighborhoodRewardTierDefinition[] VisualLevels { get; init; } = [];
}
