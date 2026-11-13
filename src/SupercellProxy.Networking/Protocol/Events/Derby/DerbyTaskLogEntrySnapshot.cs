using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby;

/// <summary>Retains the result and goal requirements of a task in the player's derby history.</summary>
public sealed record DerbyTaskLogEntrySnapshot
{
    /// <summary>Gets or sets whether this was a bingo task.</summary>
    /// <value><see langword="true"/> if the logged task counted toward the bingo board; otherwise, <see langword="false"/>.</value>
    /// <remarks>A save that omits the key reads as <see langword="false"/>.</remarks>
    public bool BingoTask { get; init; }

    /// <summary>Gets or sets whether this was a blossom task.</summary>
    /// <value><see langword="true"/> if the logged task was a blossom-variant task; otherwise, <see langword="false"/>.</value>
    public bool BlossomTask { get; init; }

    /// <summary>Gets or sets whether this was a bunny derby task.</summary>
    /// <value><see langword="true"/> if the logged task came from the bunny derby task bundles; otherwise, <see langword="false"/>.</value>
    public bool BunnyDerbyTask { get; init; }

    /// <summary>Gets or sets whether this was a chill derby task.</summary>
    /// <value><see langword="true"/> if the logged task came from the chill derby task bundles; otherwise, <see langword="false"/>.</value>
    public bool ChillDerbyTask { get; init; }

    /// <summary>Gets or sets the retained reactivation state.</summary>
    /// <value>The raw native reactivation state saved with the task, the same field the board entry carries; its values are not yet mapped, and 0 when the save omits it.</value>
    public int ReactivateStatus { get; init; }

    /// <summary>Gets or sets the older single-goal requirement when present.</summary>
    /// <value>The required count of the task's only goal in saves written before per-goal lists, or <see langword="null"/> when the save uses <see cref="RequiredQuantityA"/> instead.</value>
    public int? RequiredQuantity { get; init; }

    /// <summary>Gets or sets the required quantities for the task's goals.</summary>
    /// <value>One required count per goal, in goal order, or <see langword="null"/> when the save only has the older <see cref="RequiredQuantity"/> key.</value>
    public ImmutableArray<int>? RequiredQuantityA { get; init; }

    /// <summary>Gets or sets the task's derby points.</summary>
    /// <value>The derby points the logged task was worth.</value>
    public int RewardPoints { get; init; }

    /// <summary>Gets or sets whether the expired-task notice was seen.</summary>
    /// <value><see langword="true"/> if the player already acknowledged that the task ran out of time; otherwise, <see langword="false"/>.</value>
    public bool SeenExpiredTask { get; init; }

    /// <summary>Gets or sets whether the history entry is shown in the task stack.</summary>
    /// <value><see langword="true"/> if the log entry still appears in the visible task stack; otherwise, <see langword="false"/>.</value>
    public bool StackVisible { get; init; }

    /// <summary>Gets or sets the task definition's global data identifier.</summary>
    /// <value>The global data identifier of the neighborhood-task row, saved under the <c>NeighborhoodTaskDataID</c> key.</value>
    [JsonPropertyName("NeighborhoodTaskDataID")]
    public int TaskGlobalId { get; init; }

    /// <summary>Gets or sets the native result state for this task.</summary>
    /// <value>The raw native state that tells whether the logged task was completed, failed or discarded; the numeric values are not yet mapped and are carried through unchanged.</value>
    public int TaskStatus { get; init; }
}
