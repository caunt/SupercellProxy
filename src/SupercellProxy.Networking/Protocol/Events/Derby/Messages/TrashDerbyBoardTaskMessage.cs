using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages;

/// <summary>Requests removal of an available task from the neighborhood board.</summary>
/// <param name="NeighborhoodId">The identifier of the neighborhood whose board holds the task, written first.</param>
/// <param name="PlayerId">The account identifier of the player trashing the task.</param>
/// <param name="TaskBoardIndex">The zero-based board slot of the available task to remove.</param>
public sealed record TrashDerbyBoardTaskMessage(LongId NeighborhoodId, LongId PlayerId, int TaskBoardIndex) : CodecMessage<TrashDerbyBoardTaskMessage>, ICodecProvider<TrashDerbyBoardTaskMessage>
{
    /// <inheritdoc />
    public static ICodec<TrashDerbyBoardTaskMessage> Codec { get; } = Codecs.Complete(Codecs.Composite(Codecs.LongId,
            static message => message.NeighborhoodId,
            Codecs.LongId,
            static message => message.PlayerId,
            Codecs.VarInt,
            static message => message.TaskBoardIndex,
            static (neighborhoodId, playerId, taskBoardIndex) => new TrashDerbyBoardTaskMessage(neighborhoodId, playerId, taskBoardIndex)),
        trailingDataMessage: "The derby message has trailing data.");

    /// <summary>Omits identifiers from diagnostic text.</summary>
    /// <returns>The type name <c>TrashDerbyBoardTaskMessage</c> alone, so the neighborhood and player identifiers never reach logs.</returns>
    public override string ToString()
    {
        return nameof(TrashDerbyBoardTaskMessage);
    }
}
