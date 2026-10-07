using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands;

/// <summary>Synchronizes task metadata without reserving another task allowance.</summary>
public sealed record SynchronizeDerbyTaskServerCommand : ServerCommand
{
    /// <summary>Gets whether the task belongs to bingo derby.</summary>
    public bool BingoTask { get; init; }
    /// <summary>Gets whether the task belongs to blossom derby.</summary>
    public bool BlossomTask { get; init; }
    /// <summary>Gets whether the task belongs to bunny derby.</summary>
    public bool BunnyDerbyTask { get; init; }
    /// <summary>Gets whether the task belongs to chill derby.</summary>
    public bool ChillDerbyTask { get; init; }
    /// <summary>Gets the task timer's duration in minutes.</summary>
    public int DurationMinutes { get; init; }
    /// <summary>Gets whether the task belongs to mystery derby.</summary>
    public bool MysteryTask { get; init; }
    /// <summary>Gets the neighborhood identifier.</summary>
    public LongId NeighborhoodId { get; init; }
    /// <summary>Gets the player identifier.</summary>
    public LongId PlayerId { get; init; }
    /// <summary>Gets the task's derby points.</summary>
    public int Points { get; init; }
    /// <summary>Gets the task's reactivation state.</summary>
    public int ReactivateStatus { get; init; }
    /// <summary>Gets the required quantities in task-definition units.</summary>
    public int[] RequiredQuantities { get; init; } = [];
    /// <summary>Gets the accepted board index, or a negative value to clear the task.</summary>
    public int TaskBoardIndex { get; init; }
    /// <summary>Gets the task definition identifier.</summary>
    public int TaskGlobalId { get; init; }
    /// <inheritdoc />
    public override int Type => CommandRegistry.SynchronizeDerbyTaskServerCommandType;

    /// <summary>Decodes the task metadata before server-command metadata.</summary>
    public static SynchronizeDerbyTaskServerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new()
        {
            NeighborhoodId = stream.ReadLongId(),
            PlayerId = stream.ReadLongId(),
            TaskBoardIndex = stream.ReadVarInt(),
            TaskGlobalId = stream.ReadVarInt(),
            RequiredQuantities = stream.ReadArray(static reader => reader.ReadVarInt()),
            DurationMinutes = stream.ReadVarInt(),
            Points = stream.ReadVarInt(),
            BingoTask = stream.ReadBoolean(),
            MysteryTask = stream.ReadBoolean(),
            BlossomTask = stream.ReadBoolean(),
            ChillDerbyTask = stream.ReadBoolean(),
            BunnyDerbyTask = stream.ReadBoolean(),
            ReactivateStatus = stream.ReadVarInt(),
        };
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteLongId(NeighborhoodId);
        stream.WriteLongId(PlayerId);
        stream.WriteVarInt(TaskBoardIndex);
        stream.WriteVarInt(TaskGlobalId);
        stream.WriteArray<int>(RequiredQuantities, static (writer, value) => writer.WriteVarInt(value));
        stream.WriteVarInt(DurationMinutes);
        stream.WriteVarInt(Points);
        stream.WriteBoolean(BingoTask);
        stream.WriteBoolean(MysteryTask);
        stream.WriteBoolean(BlossomTask);
        stream.WriteBoolean(ChillDerbyTask);
        stream.WriteBoolean(BunnyDerbyTask);
        stream.WriteVarInt(ReactivateStatus);
    }
}
