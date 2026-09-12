using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>Represents the decoded FarmPassDiamondSinks JSON contract.</summary>
public sealed record FarmPassDiamondSinks
{
    /// <summary>Gets the GoalUnlockCost value.</summary>
    [JsonPropertyName("seasonGoalUnlockDiamondCost")]
    public int GoalUnlockCost { get; init; }

    /// <summary>Gets the WeeklyGoalCount value.</summary>
    [JsonPropertyName("seasonWeeklyDiamondGoalCount")]
    public int WeeklyGoalCount { get; init; }

}
