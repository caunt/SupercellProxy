using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>Diamond options of a Farm Pass season: extra weekly goals the player can unlock with diamonds.</summary>
public sealed record FarmPassDiamondSinks
{
    /// <summary>Diamonds needed to unlock the extra weekly goals; 0 when the season offers none.</summary>
    [JsonPropertyName("seasonGoalUnlockDiamondCost")]
    public int GoalUnlockCost { get; init; }

    /// <summary>How many extra weekly tasks the season adds when the unlock costs diamonds.</summary>
    [JsonPropertyName("seasonWeeklyDiamondGoalCount")]
    public int WeeklyGoalCount { get; init; }
}
