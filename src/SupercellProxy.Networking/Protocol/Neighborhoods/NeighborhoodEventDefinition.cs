using System.Collections.Immutable;
using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Neighborhoods.Rewards;

namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>Live event describing a neighbourhood event: its object, task sets, task refresh costs and reward tiers.</summary>
public sealed record NeighborhoodEventDefinition
{
    /// <summary>Gets or sets the diamonds the player pays to add one more task beyond the event's slots.</summary>
    /// <value>A diamond price per extra task, read from <c>addExtraTaskDiamondCost</c>; 0 when the event leaves it out. Extra tasks come from <see cref="ExtraTaskGroup"/>.</value>
    [JsonPropertyName("addExtraTaskDiamondCost")]
    public int ExtraTaskDiamondCost { get; init; }

    /// <summary>Gets or sets the task group used for purchased extra tasks.</summary>
    /// <value>A task group name from the neighbourhood object task table, or <see langword="null"/> when the event offers no extra tasks.</value>
    [JsonPropertyName("extraTaskGroup")]
    public string? ExtraTaskGroup { get; init; }

    /// <summary>Gets or sets the maximum number of extra active tasks.</summary>
    /// <value>How many bought tasks may be open on top of <see cref="TaskSlotCount"/>; 0 when the event allows none.</value>
    [JsonPropertyName("maxExtraTasks")]
    public int MaximumExtraTasks { get; init; }

    /// <summary>Gets or sets the personal contribution required to claim neighborhood rewards.</summary>
    /// <value>The minimum task points the player needs from the event, read from <c>minContributionToClaimRewards</c>; 0 means no minimum.</value>
    [JsonPropertyName("minContributionToClaimRewards")]
    public int MinimumContribution { get; init; }

    /// <summary>Gets or sets the neighborhood object selected for this event.</summary>
    /// <value>
    /// Saved as <c>object</c>; the row name in the neighbourhood object table of the object placed on the farm while the event runs, or
    /// <see langword="null"/> when the event names none.
    /// </value>
    [JsonPropertyName("object")]
    public string? ObjectName { get; init; }

    /// <summary>Gets or sets the diamond cost of a paid task refresh.</summary>
    /// <value>Diamonds charged for one task refresh, read from <c>taskRefreshDiamondCost</c>; 0 when the event leaves it out.</value>
    [JsonPropertyName("taskRefreshDiamondCost")]
    public int TaskRefreshDiamondCost { get; init; }

    /// <summary>Gets or sets whether the event permits advertisement-funded task refreshes.</summary>
    /// <value><see langword="true"/> if watching an ad can refresh the tasks instead of paying diamonds; otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("taskRefreshForAdWatch")]
    public bool TaskRefreshForAdWatch { get; init; }

    /// <summary>Gets or sets the task group names one task set queues, one task per group.</summary>
    /// <value>Group names from the neighbourhood object task table, read from <c>taskSet</c>; empty by default.</value>
    [JsonPropertyName("taskSet")]
    public ImmutableArray<string> TaskSet { get; init; } = [];

    /// <summary>Gets or sets how many task sets each event week grants.</summary>
    /// <value>A task-set count per event week, read from <c>taskSetsPerWeek</c>.</value>
    [JsonPropertyName("taskSetsPerWeek")]
    public int TaskSetsPerWeek { get; init; }

    /// <summary>Gets or sets how many tasks the player can have open at once.</summary>
    /// <value>The number of task slots, read from <c>taskSlotCount</c>.</value>
    [JsonPropertyName("taskSlotCount")]
    public int TaskSlotCount { get; init; }

    /// <summary>Gets or sets the configured reward entries, including their inclusive player-level ranges.</summary>
    /// <value>
    /// Entries grouped into tiers by their <see cref="NeighborhoodRewardTierDefinition.Points"/>: the distinct point values, lowest first, are the
    /// tier indices.
    /// </value>
    [JsonPropertyName("tiers")]
    public ImmutableArray<NeighborhoodRewardTierDefinition> Tiers { get; init; } = [];

    /// <summary>Gets or sets the neighborhood object's visual milestones.</summary>
    /// <value>One entry per milestone, first milestone first; each names the decoration granted and the neighbourhood points needed.</value>
    [JsonPropertyName("visualLevels")]
    public ImmutableArray<NeighborhoodRewardTierDefinition> VisualLevels { get; init; } = [];
}
