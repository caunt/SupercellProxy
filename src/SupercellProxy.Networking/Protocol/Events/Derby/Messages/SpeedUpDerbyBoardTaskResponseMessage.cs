using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages;

/// <summary>Returns a board-slot speedup result and its actual diamond charge.</summary>
public sealed record SpeedUpDerbyBoardTaskResponseMessage(LongId NeighborhoodId, LongId PlayerId, int TaskBoardIndex, int DiamondCost, int Status) : CodecMessage<SpeedUpDerbyBoardTaskResponseMessage>, ICodecProvider<SpeedUpDerbyBoardTaskResponseMessage>
{
    /// <inheritdoc />
    public static ICodec<SpeedUpDerbyBoardTaskResponseMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            Codecs.LongId,
            static message => message.NeighborhoodId,
            Codecs.LongId,
            static message => message.PlayerId,
            Codecs.VarInt,
            static message => message.TaskBoardIndex,
            Codecs.VarInt,
            static message => message.DiamondCost,
            Codecs.VarInt,
            static message => message.Status,
            static (neighborhoodId, playerId, taskBoardIndex, diamondCost, status) => new SpeedUpDerbyBoardTaskResponseMessage(neighborhoodId, playerId, taskBoardIndex, diamondCost, status)
        ),
        trailingDataMessage: "The derby message has trailing data."
    );

    /// <summary>Omits identifiers from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(SpeedUpDerbyBoardTaskResponseMessage);
    }
}
