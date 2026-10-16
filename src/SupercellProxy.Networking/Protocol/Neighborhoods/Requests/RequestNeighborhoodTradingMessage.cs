using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Requests;

/// <summary>Requests the neighborhood trading board.</summary>
/// <param name="NeighborhoodId">Neighbourhood whose board to show.</param>
/// <param name="AvatarId">Account of the player asking.</param>
public sealed record RequestNeighborhoodTradingMessage(LongId NeighborhoodId, LongId AvatarId) : CodecMessage<RequestNeighborhoodTradingMessage>, ICodecProvider<RequestNeighborhoodTradingMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestNeighborhoodTradingMessage> Codec { get; } = Codecs.Composite(
        Codecs.LongId,
        static message => message.NeighborhoodId,
        Codecs.LongId,
        static message => message.AvatarId,
        static (neighborhoodId, avatarId) => new RequestNeighborhoodTradingMessage(neighborhoodId, avatarId)
    );
}
