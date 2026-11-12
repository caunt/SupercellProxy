using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages;

/// <summary>Requests resolution of the player's selected task using its current board status.</summary>
/// <param name="TaskBoardIndex">The zero-based board slot of the task being resolved.</param>
/// <param name="TaskStatus">The raw native state the sender currently sees for that slot, the same value as <see cref="DerbyTaskEntry.Status"/>; the codes are not yet mapped.</param>
/// <param name="NeighborhoodId">The identifier of the neighborhood whose derby board holds the task.</param>
/// <param name="PlayerId">The account identifier of the player who holds the task.</param>
/// <param name="Discarded"><see langword="true"/> if the request gives the task up as a discard; otherwise, <see langword="false"/>. What native does differently for <see langword="false"/> is not yet identified.</param>
public sealed record DiscardDerbyTaskMessage(int TaskBoardIndex, int TaskStatus, LongId NeighborhoodId, LongId PlayerId, bool Discarded) : CodecMessage<DiscardDerbyTaskMessage>, ICodecProvider<DiscardDerbyTaskMessage>
{
    /// <inheritdoc />
    public static ICodec<DiscardDerbyTaskMessage> Codec { get; } = Codecs.Complete(Codecs.Composite(Codecs.VarInt,
            static message => message.TaskBoardIndex,
            Codecs.VarInt,
            static message => message.TaskStatus,
            Codecs.LongId,
            static message => message.NeighborhoodId,
            Codecs.LongId,
            static message => message.PlayerId,
            Codecs.Boolean,
            static message => message.Discarded,
            static (taskBoardIndex, taskStatus, neighborhoodId, playerId, discarded) => new DiscardDerbyTaskMessage(taskBoardIndex, taskStatus, neighborhoodId, playerId, discarded)),
        trailingDataMessage: "The derby message has trailing data.");

    /// <summary>Omits identifiers from diagnostic text.</summary>
    /// <returns>The type name <c>DiscardDerbyTaskMessage</c> alone, so the neighborhood and player identifiers never reach logs.</returns>
    public override string ToString()
    {
        return nameof(DiscardDerbyTaskMessage);
    }
}
