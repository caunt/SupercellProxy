using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Requests;

/// <summary>Requests the neighborhood trading board.</summary>
/// <param name="NeighborhoodId">The neighborhood id.</param>
/// <param name="AvatarId">The avatar id.</param>
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
