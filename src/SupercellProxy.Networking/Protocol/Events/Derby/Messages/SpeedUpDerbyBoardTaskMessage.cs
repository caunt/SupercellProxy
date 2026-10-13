using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages;

/// <summary>Requests a paid refresh of a board slot with a regeneration timer.</summary>
public sealed record SpeedUpDerbyBoardTaskMessage(int TaskBoardIndex, int DiamondCost, LongId NeighborhoodId, LongId PlayerId) : CodecMessage<SpeedUpDerbyBoardTaskMessage>, ICodecProvider<SpeedUpDerbyBoardTaskMessage>
{
    /// <inheritdoc />
    public static ICodec<SpeedUpDerbyBoardTaskMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            Codecs.VarInt,
            static message => message.TaskBoardIndex,
            Codecs.VarInt,
            static message => message.DiamondCost,
            Codecs.LongId,
            static message => message.NeighborhoodId,
            Codecs.LongId,
            static message => message.PlayerId,
            static (taskBoardIndex, diamondCost, neighborhoodId, playerId) => new SpeedUpDerbyBoardTaskMessage(taskBoardIndex, diamondCost, neighborhoodId, playerId)
        ),
        trailingDataMessage: "The derby message has trailing data."
    );

    /// <summary>Omits identifiers from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(SpeedUpDerbyBoardTaskMessage);
    }
}
