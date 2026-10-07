using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Timing;

namespace SupercellProxy.Networking.Protocol.Events.Derby;

/// <summary>Retains the avatar's active derby task, goal counters, timer and task limits.</summary>
public sealed record DerbyManagerSnapshot
{
    /// <summary>Gets whether the active task is a bingo task.</summary>
    public bool BingoTask { get; init; }
    /// <summary>Gets whether the active task is a blossom task.</summary>
    public bool BlossomTask { get; init; }
    /// <summary>Gets whether the active task is a bunny derby task.</summary>
    public bool BunnyDerbyTask { get; init; }
    /// <summary>Gets whether the active task is a chill derby task.</summary>
    public bool ChillDerbyTask { get; init; }
    /// <summary>Gets whether the task's points have reached the leaderboard.</summary>
    public bool GainedPointsLeaderboard { get; init; }
    /// <summary>Gets the definition identifier for a hot-potato task.</summary>
    public int HotPotatoTaskDataGlobalId { get; init; }
    /// <summary>Gets the board index for a hot-potato task.</summary>
    public int HotPotatoTaskIndex { get; init; }
    /// <summary>Gets the high part of the active derby instance identifier.</summary>
    public int LeagueInstanceIdHigh { get; init; }
    /// <summary>Gets the low part of the active derby instance identifier.</summary>
    public int LeagueInstanceIdLow { get; init; }
    /// <summary>Gets the current task limit supplied to the manager.</summary>
    [JsonPropertyName("MaxTasks")]
    public int MaximumTasks { get; init; }
    /// <summary>Gets whether the active task is a mystery task.</summary>
    public bool MysteryTask { get; init; }
    /// <summary>Gets the task duration before any reactivation.</summary>
    public int OriginalTaskDurationSeconds { get; init; }
    /// <summary>Gets the task's awarded derby points.</summary>
    public int Points { get; init; }
    /// <summary>Gets the older single-goal progress value when present.</summary>
    public int? Quantity { get; init; }
    /// <summary>Gets progress for each goal in the active task.</summary>
    public int[]? QuantityA { get; init; }
    /// <summary>Gets the native task-reactivation state.</summary>
    public int ReactivateStatus { get; init; }
    /// <summary>Gets the older single-goal requirement when present.</summary>
    public int? RequiredQuantity { get; init; }
    /// <summary>Gets the required quantity for each goal in the active task.</summary>
    public int[]? RequiredQuantityA { get; init; }
    /// <summary>Gets the number of task allowances already reserved.</summary>
    [JsonPropertyName("NumReservedTasks")]
    public int ReservedTaskCount { get; init; }
    /// <summary>Gets whether the expired task has been acknowledged.</summary>
    public bool SeenExpiredTask { get; init; }
    /// <summary>Gets the board slot selected by the player.</summary>
    public int SelectedTaskBoardIndex { get; init; }
    /// <summary>Gets the board index associated with the accepted task.</summary>
    public int TaskBoardIndex { get; init; }
    /// <summary>Gets whether the avatar's completion listener has been notified.</summary>
    public bool TaskCompletedNotified { get; init; }
    /// <summary>Gets the active task definition's global data identifier.</summary>
    [JsonPropertyName("NeighborhoodTaskID")]
    public int TaskGlobalId { get; init; }
    /// <summary>Gets completed, failed and discarded tasks retained in the derby task log.</summary>
    public DerbyTaskLogEntrySnapshot[] TaskLog { get; init; } = [];
    /// <summary>Gets whether the active task has no individual time limit.</summary>
    public bool TaskUnlimitedTime { get; init; }
    /// <summary>Gets the active task's native timer.</summary>
    public TimerSnapshot? Timer { get; init; }
}
