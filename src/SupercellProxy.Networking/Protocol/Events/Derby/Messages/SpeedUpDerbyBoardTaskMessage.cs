using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages;

/// <summary>Requests a paid refresh of a board slot with a regeneration timer.</summary>
/// <param name="TaskBoardIndex">The zero-based board slot whose regeneration timer should be skipped.</param>
/// <param name="DiamondCost">The diamond price the sender expects to pay; the server reports the amount it actually charged in <see cref="SpeedUpDerbyBoardTaskResponseMessage"/>.</param>
/// <param name="NeighborhoodId">The identifier of the neighborhood whose board holds the slot.</param>
/// <param name="PlayerId">The account identifier of the player paying for the refresh.</param>
public sealed record SpeedUpDerbyBoardTaskMessage(int TaskBoardIndex, int DiamondCost, LongId NeighborhoodId, LongId PlayerId) : CodecMessage<SpeedUpDerbyBoardTaskMessage>, ICodecProvider<SpeedUpDerbyBoardTaskMessage>
{
    /// <inheritdoc />
    public static ICodec<SpeedUpDerbyBoardTaskMessage> Codec { get; } = Codecs.Complete(Codecs.Composite(Codecs.VarInt,
            static message => message.TaskBoardIndex,
            Codecs.VarInt,
            static message => message.DiamondCost,
            Codecs.LongId,
            static message => message.NeighborhoodId,
            Codecs.LongId,
            static message => message.PlayerId,
            static (taskBoardIndex, diamondCost, neighborhoodId, playerId) => new SpeedUpDerbyBoardTaskMessage(taskBoardIndex, diamondCost, neighborhoodId, playerId)),
        trailingDataMessage: "The derby message has trailing data.");

    /// <summary>Omits identifiers from diagnostic text.</summary>
    /// <returns>The type name <c>SpeedUpDerbyBoardTaskMessage</c> alone, so the neighborhood and player identifiers never reach logs.</returns>
    public override string ToString()
    {
        return nameof(SpeedUpDerbyBoardTaskMessage);
    }
}
