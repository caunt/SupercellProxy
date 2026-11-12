using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages;

/// <summary>Requests task reactivation with the original task entry and retained progress.</summary>
public sealed record ReactivateDerbyTaskMessage : CodecMessage<ReactivateDerbyTaskMessage>, ICodecProvider<ReactivateDerbyTaskMessage>
{
    /// <inheritdoc />
    public static ICodec<ReactivateDerbyTaskMessage> Codec { get; } = Codecs.Complete(Codecs.Composite(Codecs.VarInt,
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
            }),
        trailingDataMessage: "The derby message has trailing data.");

    /// <summary>Gets or sets the native discard classification of the selected task.</summary>
    /// <value><see langword="true"/> if native classifies the task being reactivated as discarded; otherwise, <see langword="false"/>.</value>
    /// <remarks>It uses the same position as <see cref="DiscardDerbyTaskMessage.Discarded"/>.</remarks>
    public bool Discarded { get; init; }
    /// <summary>Gets or sets the neighborhood identifier.</summary>
    /// <value>The identifier of the neighborhood whose derby board holds the task.</value>
    public LongId NeighborhoodId { get; init; }
    /// <summary>Gets or sets the requesting player identifier.</summary>
    /// <value>The account identifier of the player asking to reactivate the task.</value>
    public LongId PlayerId { get; init; }
    /// <summary>Gets or sets the progress restored for each task goal.</summary>
    /// <value>One progress count per goal, in the same order as <see cref="DerbyTaskEntry.RequiredQuantities"/>; an empty array by default, and at most 65536 entries.</value>
    public int[] Progress { get; init; } = [];
    /// <summary>Gets or sets the selected board index.</summary>
    /// <value>The zero-based board slot of the task to reactivate, written first in the request.</value>
    public int TaskBoardIndex { get; init; }
    /// <summary>Gets or sets the original task-board entry, when present.</summary>
    /// <value>The original board entry of the task being reactivated, sent back in full, or <see langword="null"/> when its presence flag is clear.</value>
    public DerbyTaskEntry? TaskEntry { get; init; }
    /// <summary>Gets or sets the task's current native status.</summary>
    /// <value>The raw native state the sender sees for the slot, the same codes as <see cref="DerbyTaskEntry.Status"/>; they are not yet mapped to named states.</value>
    public int TaskStatus { get; init; }

    /// <summary>Omits identifiers from diagnostic text.</summary>
    /// <returns>The type name <c>ReactivateDerbyTaskMessage</c> alone, so the neighborhood, player and task-holder identifiers never reach logs.</returns>
    public override string ToString()
    {
        return nameof(ReactivateDerbyTaskMessage);
    }
}
