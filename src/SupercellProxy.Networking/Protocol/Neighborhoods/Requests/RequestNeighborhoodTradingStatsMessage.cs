using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Requests;

/// <summary>Requests a neighbor's trading statistics.</summary>
/// <param name="NeighborhoodId">Neighbourhood the member belongs to.</param>
/// <param name="MemberId">Account of the member whose statistics to show.</param>
public sealed record RequestNeighborhoodTradingStatsMessage(LongId NeighborhoodId, LongId MemberId) : CodecMessage<RequestNeighborhoodTradingStatsMessage>, ICodecProvider<RequestNeighborhoodTradingStatsMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestNeighborhoodTradingStatsMessage> Codec { get; } = Codecs.Composite(
        Codecs.LongId,
        static message => message.NeighborhoodId,
        Codecs.LongId,
        static message => message.MemberId,
        static (neighborhoodId, memberId) => new RequestNeighborhoodTradingStatsMessage(neighborhoodId, memberId)
    );
}
