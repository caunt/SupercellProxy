using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages;

/// <summary>Requests the neighborhood's current derby task board.</summary>
public sealed record RequestDerbyBoardMessage(LongId PlayerId, LongId NeighborhoodId) : CodecMessage<RequestDerbyBoardMessage>, ICodecProvider<RequestDerbyBoardMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestDerbyBoardMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            Codecs.LongId,
            static message => message.PlayerId,
            Codecs.LongId,
            static message => message.NeighborhoodId,
            static (playerId, neighborhoodId) => new RequestDerbyBoardMessage(playerId, neighborhoodId)
        ),
        trailingDataMessage: "The derby message has trailing data."
    );

    /// <summary>Omits identifiers from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(RequestDerbyBoardMessage);
    }
}
