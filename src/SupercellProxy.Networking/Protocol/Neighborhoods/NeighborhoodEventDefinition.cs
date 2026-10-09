using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>Represents the decoded NeighborhoodEventDefinition JSON contract.</summary>
public sealed record NeighborhoodEventDefinition
{
    /// <summary>Gets the diamond cost of an extra task.</summary>
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

    /// <summary>Gets the TaskSet value.</summary>
    [JsonPropertyName("taskSet")]
    public string[] TaskSet { get; init; } = [];

    /// <summary>Gets the TaskSetsPerWeek value.</summary>
    [JsonPropertyName("taskSetsPerWeek")]
    public int TaskSetsPerWeek { get; init; }

    /// <summary>Gets the TaskSlotCount value.</summary>
    [JsonPropertyName("taskSlotCount")]
    public int TaskSlotCount { get; init; }

    /// <summary>Gets the configured reward entries, including their inclusive player-level ranges.</summary>
    [JsonPropertyName("tiers")]
    public NeighborhoodRewardTierDefinition[] Tiers { get; init; } = [];

    /// <summary>Gets the neighborhood object's visual milestones.</summary>
    [JsonPropertyName("visualLevels")]
    public NeighborhoodRewardTierDefinition[] VisualLevels { get; init; } = [];
}
