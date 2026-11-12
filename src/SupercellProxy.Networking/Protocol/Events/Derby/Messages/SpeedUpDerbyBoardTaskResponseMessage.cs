using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages;

/// <summary>Returns a board-slot speedup result and its actual diamond charge.</summary>
/// <param name="NeighborhoodId">The identifier of the neighborhood whose board slot was sped up, written first.</param>
/// <param name="PlayerId">The account identifier of the player who paid for the speed-up.</param>
/// <param name="TaskBoardIndex">The zero-based board slot the result applies to, matching <see cref="SpeedUpDerbyBoardTaskMessage.TaskBoardIndex"/>.</param>
/// <param name="DiamondCost">The number of diamonds the server actually charged, which can differ from the price in the request.</param>
/// <param name="Status">The raw native result code, written last; the individual codes are not yet mapped to named outcomes.</param>
public sealed record SpeedUpDerbyBoardTaskResponseMessage(LongId NeighborhoodId, LongId PlayerId, int TaskBoardIndex, int DiamondCost, int Status) : CodecMessage<SpeedUpDerbyBoardTaskResponseMessage>, ICodecProvider<SpeedUpDerbyBoardTaskResponseMessage>
{
    /// <inheritdoc />
    public static ICodec<SpeedUpDerbyBoardTaskResponseMessage> Codec { get; } = Codecs.Complete(Codecs.Composite(Codecs.LongId,
            static message => message.NeighborhoodId,
            Codecs.LongId,
            static message => message.PlayerId,
            Codecs.VarInt,
            static message => message.TaskBoardIndex,
            Codecs.VarInt,
            static message => message.DiamondCost,
            Codecs.VarInt,
            static message => message.Status,
            static (neighborhoodId, playerId, taskBoardIndex, diamondCost, status) => new SpeedUpDerbyBoardTaskResponseMessage(neighborhoodId, playerId, taskBoardIndex, diamondCost, status)),
        trailingDataMessage: "The derby message has trailing data.");

    /// <summary>Omits identifiers from diagnostic text.</summary>
    /// <returns>The type name <c>SpeedUpDerbyBoardTaskResponseMessage</c> alone, so the neighborhood and player identifiers never reach logs.</returns>
    public override string ToString()
    {
        return nameof(SpeedUpDerbyBoardTaskResponseMessage);
    }
}
