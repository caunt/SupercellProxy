using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages;

/// <summary>Requests removal of an available task from the neighborhood board.</summary>
public sealed record TrashDerbyBoardTaskMessage(LongId NeighborhoodId, LongId PlayerId, int TaskBoardIndex) : CodecMessage<TrashDerbyBoardTaskMessage>, ICodecProvider<TrashDerbyBoardTaskMessage>
{
    /// <inheritdoc />
    public static ICodec<TrashDerbyBoardTaskMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            Codecs.LongId,
            static message => message.NeighborhoodId,
            Codecs.LongId,
            static message => message.PlayerId,
            Codecs.VarInt,
            static message => message.TaskBoardIndex,
            static (neighborhoodId, playerId, taskBoardIndex) => new TrashDerbyBoardTaskMessage(neighborhoodId, playerId, taskBoardIndex)
        ),
        trailingDataMessage: "The derby message has trailing data."
    );

    /// <summary>Omits identifiers from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(TrashDerbyBoardTaskMessage);
    }
}
