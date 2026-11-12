using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>Diamond options of a Farm Pass season: extra weekly goals the player can unlock with diamonds.</summary>
public sealed record FarmPassDiamondSinks
{
    /// <summary>Gets or sets the diamonds needed to unlock the extra weekly goals; 0 when the season offers none.</summary>
    /// <value>A diamond price read from <c>seasonGoalUnlockDiamondCost</c>.</value>
    [JsonPropertyName("seasonGoalUnlockDiamondCost")]
    public int GoalUnlockCost { get; init; }

    /// <summary>Gets or sets how many extra weekly tasks the season adds when the unlock costs diamonds.</summary>
    /// <value>A task count per weekly group, read from <c>seasonWeeklyDiamondGoalCount</c>.</value>
    [JsonPropertyName("seasonWeeklyDiamondGoalCount")]
    public int WeeklyGoalCount { get; init; }
}
