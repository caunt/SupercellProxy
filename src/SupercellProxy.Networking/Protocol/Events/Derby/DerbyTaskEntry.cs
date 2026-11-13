using System.Collections.Immutable;

using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby;

/// <summary>Preserves the complete native task-board entry carried by derby replies.</summary>
public sealed record DerbyTaskEntry : ICodecProvider<DerbyTaskEntry>
{
    /// <inheritdoc />
    public static ICodec<DerbyTaskEntry> Codec { get; } = Codecs.Composite(Codecs.OptionalLongId,
        static entry => entry.PlayerId,
        Codecs.VarInt,
        static entry => entry.TaskGlobalId,
        Codecs.VarInt,
        static entry => entry.TaskBoardIndex,
        Codecs.VarInt,
        static entry => entry.Unknown1,
        Codecs.VarInt,
        static entry => entry.Status,
        Codecs.Array(Codecs.VarInt, CollectionCount.Bounded),
        static entry => entry.RequiredQuantities,
        Codecs.VarInt,
        static entry => entry.RewardPoints,
        Codecs.VarInt,
        static entry => entry.DurationMinutes,
        Codecs.VarInt,
        static entry => entry.Unknown3,
        Codecs.VarInt,
        static entry => entry.Unknown4,
        TaskKinds.Codec,
        static entry => new TaskKinds(entry.BingoTask, entry.MysteryTask, entry.BlossomTask, entry.ChillDerbyTask, entry.BunnyDerbyTask),
        Codecs.VarInt,
        static entry => entry.HotPotatoTaskDataGlobalId,
        Codecs.VarInt,
        static entry => entry.HotPotatoTaskIndex,
        Codecs.Array(Codecs.LongId, CollectionCount.Bounded),
        static entry => entry.RelatedPlayerIds,
        TaskTail.Codec,
        static entry => new TaskTail(entry.SeenExpiredTask, entry.Unknown7, entry.Unknown8, entry.Unknown9, entry.StackVisible, entry.ReactivateStatus),
        static (playerId,
            taskGlobalId,
            taskBoardIndex,
            unknown1,
            status,
            requiredQuantities,
            rewardPoints,
            durationMinutes,
            unknown3,
            unknown4,
            taskKinds,
            hotPotatoTaskDataGlobalId,
            hotPotatoTaskIndex,
            relatedPlayerIds,
            taskTail) => new DerbyTaskEntry
            {
                PlayerId = playerId,
                TaskGlobalId = taskGlobalId,
                TaskBoardIndex = taskBoardIndex,
                Unknown1 = unknown1,
                Status = status,
                RequiredQuantities = requiredQuantities,
                RewardPoints = rewardPoints,
                DurationMinutes = durationMinutes,
                Unknown3 = unknown3,
                Unknown4 = unknown4,
                BingoTask = taskKinds.BingoTask,
                MysteryTask = taskKinds.MysteryTask,
                BlossomTask = taskKinds.BlossomTask,
                ChillDerbyTask = taskKinds.ChillDerbyTask,
                BunnyDerbyTask = taskKinds.BunnyDerbyTask,
                HotPotatoTaskDataGlobalId = hotPotatoTaskDataGlobalId,
                HotPotatoTaskIndex = hotPotatoTaskIndex,
                RelatedPlayerIds = relatedPlayerIds,
                SeenExpiredTask = taskTail.SeenExpiredTask,
                Unknown7 = taskTail.Unknown7,
                Unknown8 = taskTail.Unknown8,
                Unknown9 = taskTail.Unknown9,
                StackVisible = taskTail.StackVisible,
                ReactivateStatus = taskTail.ReactivateStatus,
            });

    /// <summary>Gets or sets whether this is a bingo task.</summary>
    /// <value><see langword="true"/> if native marks the task as counting toward the bingo board; otherwise, <see langword="false"/>.</value>
    /// <remarks>It is the first of the five task-kind flags on the wire.</remarks>
    public bool BingoTask { get; init; }

    /// <summary>Gets or sets whether this is a blossom task.</summary>
    /// <value><see langword="true"/> if the entry is a blossom-variant task; otherwise, <see langword="false"/>.</value>
    /// <remarks>It is the third task-kind flag, after <see cref="MysteryTask"/>.</remarks>
    public bool BlossomTask { get; init; }

    /// <summary>Gets or sets whether this is a bunny derby task.</summary>
    /// <value><see langword="true"/> if the entry belongs to the bunny derby task bundles; otherwise, <see langword="false"/>.</value>
    /// <remarks>It is the last task-kind flag on the wire.</remarks>
    public bool BunnyDerbyTask { get; init; }

    /// <summary>Gets or sets whether this is a chill derby task.</summary>
    /// <value><see langword="true"/> if the entry belongs to the chill derby task bundles; otherwise, <see langword="false"/>.</value>
    public bool ChillDerbyTask { get; init; }

    /// <summary>Gets or sets the task's duration in minutes.</summary>
    /// <value>How long a player has to finish the task once taken, in whole minutes, as the var-int written after <see cref="RewardPoints"/>.</value>
    public int DurationMinutes { get; init; }

    /// <summary>Gets or sets the linked hot-potato task definition.</summary>
    /// <value>The global data identifier of the task definition this entry is chained to in a hot-potato derby; native writes it even when the entry is not a hot-potato task.</value>
    public int HotPotatoTaskDataGlobalId { get; init; }

    /// <summary>Gets or sets the linked hot-potato board index.</summary>
    /// <value>The board slot of the hot-potato task linked to <see cref="HotPotatoTaskDataGlobalId"/>, as the var-int read right after it.</value>
    public int HotPotatoTaskIndex { get; init; }

    /// <summary>Gets or sets whether this is a mystery task.</summary>
    /// <value><see langword="true"/> if native flags the entry as a mystery task; otherwise, <see langword="false"/>.</value>
    /// <remarks>It is the second task-kind flag.</remarks>
    public bool MysteryTask { get; init; }

    /// <summary>Gets or sets the player associated with this board entry, when assigned.</summary>
    /// <value>The account identifier of the neighborhood member who holds the task, or <see langword="null"/> when the presence flag before it is clear and the slot is free.</value>
    public LongId? PlayerId { get; init; }

    /// <summary>Gets or sets the task-reactivation state.</summary>
    /// <value>The raw native reactivation state written last in the entry; its individual values are not yet mapped, and 0 is the default.</value>
    public int ReactivateStatus { get; init; }

    /// <summary>Gets or sets the related player identifiers in native order.</summary>
    /// <value>The account identifiers native lists with the task, kept in the order they were read; an empty array by default.</value>
    public ImmutableArray<LongId> RelatedPlayerIds { get; init; } = [];

    /// <summary>Gets or sets the quantities required by the task's goals.</summary>
    /// <value>One required count per goal, in goal order; an empty array when the entry carries no goals, and at most 65536 entries.</value>
    public ImmutableArray<int> RequiredQuantities { get; init; } = [];

    /// <summary>Gets or sets the points offered for the task.</summary>
    /// <value>The derby points credited to the neighborhood when the task is completed.</value>
    public int RewardPoints { get; init; }

    /// <summary>Gets or sets whether the expired-task notice has been seen.</summary>
    /// <value><see langword="true"/> if the holder has already acknowledged that the task ran out of time; otherwise, <see langword="false"/>.</value>
    public bool SeenExpiredTask { get; init; }

    /// <summary>Gets or sets whether this entry is shown in the task stack.</summary>
    /// <value><see langword="true"/> if the board lists the entry in its visible task stack; otherwise, <see langword="false"/>.</value>
    public bool StackVisible { get; init; }

    /// <summary>Gets or sets the native task state.</summary>
    /// <value>The raw native state of the board slot, written after <see cref="Unknown1"/>; the numeric values are not yet mapped to named states and are passed through unchanged.</value>
    public int Status { get; init; }

    /// <summary>Gets or sets the task's index on the neighborhood board.</summary>
    /// <value>The zero-based slot the entry occupies on the shared derby board.</value>
    public int TaskBoardIndex { get; init; }

    /// <summary>Gets or sets the neighborhood-task definition identifier.</summary>
    /// <value>The global data identifier of the neighborhood-task row that defines the task's goals and rules.</value>
    public int TaskGlobalId { get; init; }

    /// <summary>Gets or sets the second retained board value.</summary>
    /// <value>The var-int written between <see cref="TaskBoardIndex"/> and <see cref="Status"/>; its meaning is not yet identified, so it is carried through unchanged.</value>
    public int Unknown1 { get; init; }

    /// <summary>Gets or sets the next retained task value.</summary>
    /// <value>The var-int written right after <see cref="DurationMinutes"/>; its meaning is not yet identified.</value>
    public int Unknown3 { get; init; }

    /// <summary>Gets or sets the retained task value preceding variant flags.</summary>
    /// <value>The var-int written just before the five task-kind flags; its meaning is not yet identified, and it defaults to -1.</value>
    public int Unknown4 { get; init; } = -1;

    /// <summary>Gets or sets the first retained value following expiry acknowledgement.</summary>
    /// <value>The var-int written right after <see cref="SeenExpiredTask"/>; its meaning is not yet identified.</value>
    public int Unknown7 { get; init; }

    /// <summary>Gets or sets the next retained task value.</summary>
    /// <value>The var-int written between <see cref="Unknown7"/> and <see cref="Unknown9"/>; its meaning is not yet identified.</value>
    public int Unknown8 { get; init; }

    /// <summary>Gets or sets the final retained value before stack visibility.</summary>
    /// <value>The var-int written just before <see cref="StackVisible"/>; its meaning is not yet identified, and it defaults to -1.</value>
    public int Unknown9 { get; init; } = -1;

    // The task kind flags native writes together.
    private readonly record struct TaskKinds(bool BingoTask, bool MysteryTask, bool BlossomTask, bool ChillDerbyTask, bool BunnyDerbyTask)
    {
        public static ICodec<TaskKinds> Codec { get; } = Codecs.Composite(Codecs.Boolean,
            static segment => segment.BingoTask,
            Codecs.Boolean,
            static segment => segment.MysteryTask,
            Codecs.Boolean,
            static segment => segment.BlossomTask,
            Codecs.Boolean,
            static segment => segment.ChillDerbyTask,
            Codecs.Boolean,
            static segment => segment.BunnyDerbyTask,
            static (bingoTask, mysteryTask, blossomTask, chillDerbyTask, bunnyDerbyTask) => new TaskKinds(bingoTask, mysteryTask, blossomTask, chillDerbyTask, bunnyDerbyTask));
    }

    // The trailing task state, read as one group to keep the field list within a composite.
    private readonly record struct TaskTail(bool SeenExpiredTask, int Unknown7, int Unknown8, int Unknown9, bool StackVisible, int ReactivateStatus)
    {
        public static ICodec<TaskTail> Codec { get; } = Codecs.Composite(Codecs.Boolean,
            static segment => segment.SeenExpiredTask,
            Codecs.VarInt,
            static segment => segment.Unknown7,
            Codecs.VarInt,
            static segment => segment.Unknown8,
            Codecs.VarInt,
            static segment => segment.Unknown9,
            Codecs.Boolean,
            static segment => segment.StackVisible,
            Codecs.VarInt,
            static segment => segment.ReactivateStatus,
            static (seenExpiredTask, unknown7, unknown8, unknown9, stackVisible, reactivateStatus) => new TaskTail(seenExpiredTask, unknown7, unknown8, unknown9, stackVisible, reactivateStatus));
    }
}
