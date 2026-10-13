using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby;

/// <summary>Preserves the complete native task-board entry carried by derby replies.</summary>
public sealed record DerbyTaskEntry : ICodecProvider<DerbyTaskEntry>
{
    /// <inheritdoc />
    public static ICodec<DerbyTaskEntry> Codec { get; } = Codecs.Composite(
        Codecs.OptionalLongId,
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
        static (
            playerId,
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
            taskTail
        ) => new DerbyTaskEntry
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
        }
    );

    /// <summary>Gets whether this is a bingo task.</summary>
    public bool BingoTask { get; init; }
    /// <summary>Gets whether this is a blossom task.</summary>
    public bool BlossomTask { get; init; }
    /// <summary>Gets whether this is a bunny derby task.</summary>
    public bool BunnyDerbyTask { get; init; }
    /// <summary>Gets whether this is a chill derby task.</summary>
    public bool ChillDerbyTask { get; init; }
    /// <summary>Gets the task's duration in minutes.</summary>
    public int DurationMinutes { get; init; }
    /// <summary>Gets the linked hot-potato task definition.</summary>
    public int HotPotatoTaskDataGlobalId { get; init; }
    /// <summary>Gets the linked hot-potato board index.</summary>
    public int HotPotatoTaskIndex { get; init; }
    /// <summary>Gets whether this is a mystery task.</summary>
    public bool MysteryTask { get; init; }
    /// <summary>Gets the player associated with this board entry, when assigned.</summary>
    public LongId? PlayerId { get; init; }
    /// <summary>Gets the task-reactivation state.</summary>
    public int ReactivateStatus { get; init; }
    /// <summary>Gets the quantities required by the task's goals.</summary>
    public int[] RequiredQuantities { get; init; } = [];
    /// <summary>Gets the related player identifiers in native order.</summary>
    public LongId[] RelatedPlayerIds { get; init; } = [];
    /// <summary>Gets the points offered for the task.</summary>
    public int RewardPoints { get; init; }
    /// <summary>Gets whether the expired-task notice has been seen.</summary>
    public bool SeenExpiredTask { get; init; }
    /// <summary>Gets whether this entry is shown in the task stack.</summary>
    public bool StackVisible { get; init; }
    /// <summary>Gets the native task state.</summary>
    public int Status { get; init; }
    /// <summary>Gets the task's index on the neighborhood board.</summary>
    public int TaskBoardIndex { get; init; }
    /// <summary>Gets the neighborhood-task definition identifier.</summary>
    public int TaskGlobalId { get; init; }
    /// <summary>Gets the second retained board value.</summary>
    public int Unknown1 { get; init; }
    /// <summary>Gets the next retained task value.</summary>
    public int Unknown3 { get; init; }
    /// <summary>Gets the retained task value preceding variant flags.</summary>
    public int Unknown4 { get; init; } = -1;
    /// <summary>Gets the first retained value following expiry acknowledgement.</summary>
    public int Unknown7 { get; init; }
    /// <summary>Gets the next retained task value.</summary>
    public int Unknown8 { get; init; }
    /// <summary>Gets the final retained value before stack visibility.</summary>
    public int Unknown9 { get; init; } = -1;

    // The task kind flags native writes together.
    private readonly record struct TaskKinds(bool BingoTask, bool MysteryTask, bool BlossomTask, bool ChillDerbyTask, bool BunnyDerbyTask)
    {
        public static ICodec<TaskKinds> Codec { get; } = Codecs.Composite(
            Codecs.Boolean,
            static segment => segment.BingoTask,
            Codecs.Boolean,
            static segment => segment.MysteryTask,
            Codecs.Boolean,
            static segment => segment.BlossomTask,
            Codecs.Boolean,
            static segment => segment.ChillDerbyTask,
            Codecs.Boolean,
            static segment => segment.BunnyDerbyTask,
            static (bingoTask, mysteryTask, blossomTask, chillDerbyTask, bunnyDerbyTask) => new TaskKinds(bingoTask, mysteryTask, blossomTask, chillDerbyTask, bunnyDerbyTask)
        );
    }

    // The trailing task state, read as one group to keep the field list within a composite.
    private readonly record struct TaskTail(bool SeenExpiredTask, int Unknown7, int Unknown8, int Unknown9, bool StackVisible, int ReactivateStatus)
    {
        public static ICodec<TaskTail> Codec { get; } = Codecs.Composite(
            Codecs.Boolean,
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
            static (seenExpiredTask, unknown7, unknown8, unknown9, stackVisible, reactivateStatus) => new TaskTail(seenExpiredTask, unknown7, unknown8, unknown9, stackVisible, reactivateStatus)
        );
    }
}
