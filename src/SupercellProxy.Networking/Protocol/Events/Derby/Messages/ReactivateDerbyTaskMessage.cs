using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages;

/// <summary>Requests task reactivation with the original task entry and retained progress.</summary>
public sealed record ReactivateDerbyTaskMessage : CodecMessage<ReactivateDerbyTaskMessage>, ICodecProvider<ReactivateDerbyTaskMessage>
{
    /// <inheritdoc />
    public static ICodec<ReactivateDerbyTaskMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            Codecs.VarInt,
            static message => message.TaskBoardIndex,
            Codecs.VarInt,
            static message => message.TaskStatus,
            Codecs.LongId,
            static message => message.NeighborhoodId,
            Codecs.LongId,
            static message => message.PlayerId,
            Codecs.Boolean,
            static message => message.Discarded,
            Codecs.Optional(DerbyTaskEntry.Codec),
            static message => message.TaskEntry,
            Codecs.Array(Codecs.VarInt, CollectionCount.Bounded),
            static message => message.Progress,
            static (taskBoardIndex, taskStatus, neighborhoodId, playerId, discarded, taskEntry, progress) => new ReactivateDerbyTaskMessage
            {
                TaskBoardIndex = taskBoardIndex,
                TaskStatus = taskStatus,
                NeighborhoodId = neighborhoodId,
                PlayerId = playerId,
                Discarded = discarded,
                TaskEntry = taskEntry,
                Progress = progress,
            }
        ),
        trailingDataMessage: "The derby message has trailing data."
    );

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

    /// <summary>Omits identifiers from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(ReactivateDerbyTaskMessage);
    }
}
