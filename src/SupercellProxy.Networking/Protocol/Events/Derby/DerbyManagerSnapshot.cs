using System.Collections.Immutable;
using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Timing;

namespace SupercellProxy.Networking.Protocol.Events.Derby;

/// <summary>Retains the avatar's active derby task, goal counters, timer and task limits.</summary>
public sealed record DerbyManagerSnapshot
{
    /// <summary>Gets or sets whether the active task is a bingo task.</summary>
    /// <value><see langword="true"/> if the active task counts toward the bingo board; otherwise, <see langword="false"/>.</value>
    public bool BingoTask { get; init; }

    /// <summary>Gets or sets whether the active task is a blossom task.</summary>
    /// <value><see langword="true"/> if the active task belongs to a blossom derby; otherwise, <see langword="false"/>.</value>
    public bool BlossomTask { get; init; }

    /// <summary>Gets or sets whether the active task is a bunny derby task.</summary>
    /// <value><see langword="true"/> if the active task belongs to a bunny derby; otherwise, <see langword="false"/>.</value>
    public bool BunnyDerbyTask { get; init; }

    /// <summary>Gets or sets whether the active task is a chill derby task.</summary>
    /// <value><see langword="true"/> if the active task belongs to a chill derby; otherwise, <see langword="false"/>.</value>
    public bool ChillDerbyTask { get; init; }

    /// <summary>Gets or sets whether the task's points have reached the leaderboard.</summary>
    /// <value><see langword="true"/> if the completed task's leaderboard points have been recorded; otherwise, <see langword="false"/>.</value>
    public bool GainedPointsLeaderboard { get; init; }

    /// <summary>Gets or sets the definition identifier for a hot-potato task.</summary>
    /// <value>The data-table global id of the hot-potato task linked to the active task.</value>
    public int HotPotatoTaskDataGlobalId { get; init; }

    /// <summary>Gets or sets the board index for a hot-potato task.</summary>
    /// <value>The index of the hot-potato task linked to the active task.</value>
    public int HotPotatoTaskIndex { get; init; }

    /// <summary>Gets or sets the high part of the active derby instance identifier.</summary>
    /// <value>The high 32 bits of the derby instance the task log belongs to, or -1 when no instance is recorded.</value>
    public int LeagueInstanceIdHigh { get; init; }

    /// <summary>Gets or sets the low part of the active derby instance identifier.</summary>
    /// <value>The low 32 bits of that derby instance, or -1 when no instance is recorded.</value>
    public int LeagueInstanceIdLow { get; init; }

    /// <summary>Gets or sets the current task limit supplied to the manager.</summary>
    /// <value>How many tasks the player may take in the current derby (saved as <c>MaxTasks</c>).</value>
    [JsonPropertyName("MaxTasks")]
    public int MaximumTasks { get; init; }

    /// <summary>Gets or sets whether the active task is a mystery task.</summary>
    /// <value><see langword="true"/> if the active task is a mystery task; otherwise, <see langword="false"/>.</value>
    public bool MysteryTask { get; init; }

    /// <summary>Gets or sets the task duration before any reactivation.</summary>
    /// <value>The length in seconds the active task's timer started with; 0 when no task is active.</value>
    public int OriginalTaskDurationSeconds { get; init; }

    /// <summary>Gets or sets the task's awarded derby points.</summary>
    /// <value>The points the active task is worth; 0 when no task is active.</value>
    public int Points { get; init; }

    /// <summary>Gets or sets the older single-goal progress value when present.</summary>
    /// <value>The progress of a single-goal task in older saves, or <see langword="null"/> when the save uses <see cref="QuantityA"/> instead.</value>
    public int? Quantity { get; init; }

    /// <summary>Gets or sets progress for each goal in the active task.</summary>
    /// <value>One progress counter per goal, parallel to <see cref="RequiredQuantityA"/>; empty or <see langword="null"/> when no task is active.</value>
    public ImmutableArray<int>? QuantityA { get; init; }

    /// <summary>Gets or sets the native task-reactivation state.</summary>
    /// <value>The reactivation status supplied with the active task, where 1 marks a reactivated task; 0 when no task is active.</value>
    public int ReactivateStatus { get; init; }

    /// <summary>Gets or sets the older single-goal requirement when present.</summary>
    /// <value>The target of a single-goal task in older saves, or <see langword="null"/> when the save uses <see cref="RequiredQuantityA"/> instead.</value>
    public int? RequiredQuantity { get; init; }

    /// <summary>Gets or sets the required quantity for each goal in the active task.</summary>
    /// <value>One target per goal, with fish-weight goals stored multiplied by 1000; empty or <see langword="null"/> when no task is active.</value>
    public ImmutableArray<int>? RequiredQuantityA { get; init; }

    /// <summary>Gets or sets the number of task allowances already reserved.</summary>
    /// <value>How many tasks the player has taken in the current derby instance (saved as <c>NumReservedTasks</c>).</value>
    [JsonPropertyName("NumReservedTasks")]
    public int ReservedTaskCount { get; init; }

    /// <summary>Gets or sets whether the expired task has been acknowledged.</summary>
    /// <value><see langword="true"/> if the player has seen the active task expire; otherwise, <see langword="false"/>.</value>
    public bool SeenExpiredTask { get; init; }

    /// <summary>Gets or sets the board slot selected by the player.</summary>
    /// <value>The task-board slot the player picked when asking for the active task, or -1 when no task is active.</value>
    public int SelectedTaskBoardIndex { get; init; }

    /// <summary>Gets or sets the board index associated with the accepted task.</summary>
    /// <value>The task-board slot the active task occupies, or -1 when no task is active.</value>
    public int TaskBoardIndex { get; init; }

    /// <summary>Gets or sets whether the active task's completion has already been handled.</summary>
    /// <value><see langword="true"/> if the completion of the active task has been reported; otherwise, <see langword="false"/>.</value>
    public bool TaskCompletedNotified { get; init; }

    /// <summary>Gets or sets the active task definition's global data identifier.</summary>
    /// <value>The global id of the active task's row in the neighbourhood tasks table (saved as <c>NeighborhoodTaskID</c>), or -1 when no task is active.</value>
    [JsonPropertyName("NeighborhoodTaskID")]
    public int TaskGlobalId { get; init; }

    /// <summary>Gets or sets completed, failed and discarded tasks retained in the derby task log.</summary>
    /// <value>The log entries of the current derby instance in the order they were added.</value>
    public ImmutableArray<DerbyTaskLogEntrySnapshot> TaskLog { get; init; } = [];

    /// <summary>Gets or sets whether the active task has no individual time limit.</summary>
    /// <value><see langword="true"/> if the active task was started with a duration of 0 or less; otherwise, <see langword="false"/>.</value>
    public bool TaskUnlimitedTime { get; init; }

    /// <summary>Gets or sets the active task's native timer.</summary>
    /// <value>The task's countdown, holding its length in seconds and the ticks left at 15 ticks per second, or <see langword="null"/> when no task is active.</value>
    public TimerSnapshot? Timer { get; init; }
}
