using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby;

/// <summary>Preserves the complete native task-board entry carried by derby replies.</summary>
public sealed record DerbyTaskEntry
{
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

    /// <summary>Decodes one native task-board entry.</summary>
    public static DerbyTaskEntry Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new()
        {
            PlayerId = stream.ReadOptionalLongId(),
            TaskGlobalId = stream.ReadVarInt(),
            TaskBoardIndex = stream.ReadVarInt(),
            Unknown1 = stream.ReadVarInt(),
            Status = stream.ReadVarInt(),
            RequiredQuantities = stream.ReadArray(static reader => reader.ReadVarInt()),
            RewardPoints = stream.ReadVarInt(),
            DurationMinutes = stream.ReadVarInt(),
            Unknown3 = stream.ReadVarInt(),
            Unknown4 = stream.ReadVarInt(),
            BingoTask = stream.ReadBoolean(),
            MysteryTask = stream.ReadBoolean(),
            BlossomTask = stream.ReadBoolean(),
            ChillDerbyTask = stream.ReadBoolean(),
            BunnyDerbyTask = stream.ReadBoolean(),
            HotPotatoTaskDataGlobalId = stream.ReadVarInt(),
            HotPotatoTaskIndex = stream.ReadVarInt(),
            RelatedPlayerIds = stream.ReadArray(static reader => reader.ReadLongId()),
            SeenExpiredTask = stream.ReadBoolean(),
            Unknown7 = stream.ReadVarInt(),
            Unknown8 = stream.ReadVarInt(),
            Unknown9 = stream.ReadVarInt(),
            StackVisible = stream.ReadBoolean(),
            ReactivateStatus = stream.ReadVarInt(),
        };
    }

    /// <summary>Encodes the entry in native field order.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.WriteOptionalLongId(PlayerId);
        stream.WriteVarInt(TaskGlobalId);
        stream.WriteVarInt(TaskBoardIndex);
        stream.WriteVarInt(Unknown1);
        stream.WriteVarInt(Status);
        stream.WriteArray<int>(RequiredQuantities, static (writer, value) => writer.WriteVarInt(value));
        stream.WriteVarInt(RewardPoints);
        stream.WriteVarInt(DurationMinutes);
        stream.WriteVarInt(Unknown3);
        stream.WriteVarInt(Unknown4);
        stream.WriteBoolean(BingoTask);
        stream.WriteBoolean(MysteryTask);
        stream.WriteBoolean(BlossomTask);
        stream.WriteBoolean(ChillDerbyTask);
        stream.WriteBoolean(BunnyDerbyTask);
        stream.WriteVarInt(HotPotatoTaskDataGlobalId);
        stream.WriteVarInt(HotPotatoTaskIndex);
        stream.WriteArray<LongId>(RelatedPlayerIds, static (writer, value) => writer.WriteLongId(value));
        stream.WriteBoolean(SeenExpiredTask);
        stream.WriteVarInt(Unknown7);
        stream.WriteVarInt(Unknown8);
        stream.WriteVarInt(Unknown9);
        stream.WriteBoolean(StackVisible);
        stream.WriteVarInt(ReactivateStatus);
    }
}
