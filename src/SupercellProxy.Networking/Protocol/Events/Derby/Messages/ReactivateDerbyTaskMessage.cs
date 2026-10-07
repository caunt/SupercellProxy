using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages;

/// <summary>Requests task reactivation with the original task entry and retained progress.</summary>
public sealed record ReactivateDerbyTaskMessage : IMessage
{
    /// <summary>Gets the native discard classification of the selected task.</summary>
    public bool Discarded { get; init; }
    /// <summary>Gets the neighborhood identifier.</summary>
    public LongId NeighborhoodId { get; init; }
    /// <summary>Gets the requesting player identifier.</summary>
    public LongId PlayerId { get; init; }
    /// <summary>Gets the progress restored for each task goal.</summary>
    public int[] Progress { get; init; } = [];
    /// <summary>Gets the selected board index.</summary>
    public int TaskBoardIndex { get; init; }
    /// <summary>Gets the original task-board entry, when present.</summary>
    public DerbyTaskEntry? TaskEntry { get; init; }
    /// <summary>Gets the task's current native status.</summary>
    public int TaskStatus { get; init; }

    /// <summary>Decodes the task reactivation request.</summary>
    public static ReactivateDerbyTaskMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        ReactivateDerbyTaskMessage message = new()
        {
            TaskBoardIndex = stream.ReadVarInt(),
            TaskStatus = stream.ReadVarInt(),
            NeighborhoodId = stream.ReadLongId(),
            PlayerId = stream.ReadLongId(),
            Discarded = stream.ReadBoolean(),
            TaskEntry = stream.ReadBoolean() ? DerbyTaskEntry.Decode(stream) : null,
            Progress = stream.ReadArray(static reader => reader.ReadVarInt()),
        };

        DerbyMessageCodec.RequireEnd(stream);

        return message;
    }

    /// <summary>Encodes the original entry before its retained progress.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVarInt(TaskBoardIndex);
        stream.WriteVarInt(TaskStatus);
        stream.WriteLongId(NeighborhoodId);
        stream.WriteLongId(PlayerId);
        stream.WriteBoolean(Discarded);
        stream.WriteBoolean(TaskEntry is not null);
        TaskEntry?.Encode(stream);
        stream.WriteArray<int>(Progress, static (writer, value) => writer.WriteVarInt(value));
    }

    /// <summary>Omits identifiers from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(ReactivateDerbyTaskMessage);
    }
}
