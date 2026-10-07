using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby;

/// <summary>Retains the result and goal requirements of a task in the player's derby history.</summary>
public sealed record DerbyTaskLogEntrySnapshot
{
    /// <summary>Gets whether this was a bingo task.</summary>
    public bool BingoTask { get; init; }
    /// <summary>Gets whether this was a blossom task.</summary>
    public bool BlossomTask { get; init; }
    /// <summary>Gets whether this was a bunny derby task.</summary>
    public bool BunnyDerbyTask { get; init; }
    /// <summary>Gets whether this was a chill derby task.</summary>
    public bool ChillDerbyTask { get; init; }
    /// <summary>Gets the retained reactivation state.</summary>
    public int ReactivateStatus { get; init; }
    /// <summary>Gets the older single-goal requirement when present.</summary>
    public int? RequiredQuantity { get; init; }
    /// <summary>Gets the required quantities for the task's goals.</summary>
    public int[]? RequiredQuantityA { get; init; }
    /// <summary>Gets the task's derby points.</summary>
    public int RewardPoints { get; init; }
    /// <summary>Gets whether the expired-task notice was seen.</summary>
    public bool SeenExpiredTask { get; init; }
    /// <summary>Gets whether the history entry is shown in the task stack.</summary>
    public bool StackVisible { get; init; }
    /// <summary>Gets the task definition's global data identifier.</summary>
    [JsonPropertyName("NeighborhoodTaskDataID")]
    public int TaskGlobalId { get; init; }
    /// <summary>Gets the native result state for this task.</summary>
    public int TaskStatus { get; init; }
}
