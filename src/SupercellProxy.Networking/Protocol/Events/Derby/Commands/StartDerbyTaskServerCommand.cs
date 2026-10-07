using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands;

/// <summary>Reports the result of taking or reactivating a task and supplies its authoritative goals.</summary>
public sealed record StartDerbyTaskServerCommand : ServerCommand
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
    /// <summary>Gets the accompanying board-entry update, when supplied.</summary>
    public DerbyTaskEntry? Entry { get; init; }
    /// <summary>Gets the linked hot-potato definition identifier.</summary>
    public int HotPotatoTaskDataGlobalId { get; init; }
    /// <summary>Gets the linked hot-potato task index.</summary>
    public int HotPotatoTaskIndex { get; init; }
    /// <summary>Gets the player's task allowance.</summary>
    public int MaximumTasks { get; init; }
    /// <summary>Gets whether this is a mystery task.</summary>
    public bool MysteryTask { get; init; }
    /// <summary>Gets the neighborhood to which the reply belongs.</summary>
    public LongId NeighborhoodId { get; init; }
    /// <summary>Gets the player taking the task.</summary>
    public LongId PlayerId { get; init; }
    /// <summary>Gets the task's derby points.</summary>
    public int Points { get; init; }
    /// <summary>Gets the required quantities for each goal.</summary>
    public int[] RequiredQuantities { get; init; } = [];
    /// <summary>Gets the reactivation state supplied with the task.</summary>
    public int ReactivateStatus { get; init; }
    /// <summary>Gets restored progress for the accepted task.</summary>
    public int[] Progress { get; init; } = [];
    /// <summary>Gets the requested board slot.</summary>
    public int SelectedTaskBoardIndex { get; init; }
    /// <summary>Gets the native request result; one accepts the task.</summary>
    public int Status { get; init; }
    /// <summary>Gets the accepted task's board slot.</summary>
    public int TaskBoardIndex { get; init; }
    /// <summary>Gets the task definition's global data identifier.</summary>
    public int TaskGlobalId { get; init; }
    /// <inheritdoc />
    public override int Type => CommandRegistry.StartDerbyTaskServerCommandType;

    /// <summary>Decodes the reply before its server-command metadata.</summary>
    public static StartDerbyTaskServerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new()
        {
            NeighborhoodId = stream.ReadLongId(),
            PlayerId = stream.ReadLongId(),
            SelectedTaskBoardIndex = stream.ReadVarInt(),
            TaskBoardIndex = stream.ReadVarInt(),
            TaskGlobalId = stream.ReadVarInt(),
            RequiredQuantities = stream.ReadArray(static reader => reader.ReadVarInt()),
            DurationMinutes = stream.ReadVarInt(),
            Points = stream.ReadVarInt(),
            Status = stream.ReadVarInt(),
            BingoTask = stream.ReadBoolean(),
            MysteryTask = stream.ReadBoolean(),
            BlossomTask = stream.ReadBoolean(),
            ChillDerbyTask = stream.ReadBoolean(),
            BunnyDerbyTask = stream.ReadBoolean(),
            HotPotatoTaskDataGlobalId = stream.ReadVarInt(),
            HotPotatoTaskIndex = stream.ReadVarInt(),
            MaximumTasks = stream.ReadVarInt(),
            ReactivateStatus = stream.ReadVarInt(),
            Entry = stream.ReadBoolean() ? DerbyTaskEntry.Decode(stream) : null,
            Progress = stream.ReadArray(static reader => reader.ReadVarInt()),
        };
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteLongId(NeighborhoodId);
        stream.WriteLongId(PlayerId);
        stream.WriteVarInt(SelectedTaskBoardIndex);
        stream.WriteVarInt(TaskBoardIndex);
        stream.WriteVarInt(TaskGlobalId);
        stream.WriteArray<int>(RequiredQuantities, static (writer, value) => writer.WriteVarInt(value));
        stream.WriteVarInt(DurationMinutes);
        stream.WriteVarInt(Points);
        stream.WriteVarInt(Status);
        stream.WriteBoolean(BingoTask);
        stream.WriteBoolean(MysteryTask);
        stream.WriteBoolean(BlossomTask);
        stream.WriteBoolean(ChillDerbyTask);
        stream.WriteBoolean(BunnyDerbyTask);
        stream.WriteVarInt(HotPotatoTaskDataGlobalId);
        stream.WriteVarInt(HotPotatoTaskIndex);
        stream.WriteVarInt(MaximumTasks);
        stream.WriteVarInt(ReactivateStatus);
        stream.WriteBoolean(Entry is not null);
        Entry?.Encode(stream);
        stream.WriteArray<int>(Progress, static (writer, value) => writer.WriteVarInt(value));
    }
}
