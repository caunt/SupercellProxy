using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Objects;

/// <summary>Requests the leaderboard of a neighborhood object.</summary>
/// <param name="NeighborhoodId">The neighborhood id.</param>
public sealed record RequestNeighborhoodObjectLeaderboardMessage(LongId? NeighborhoodId) : CodecMessage<RequestNeighborhoodObjectLeaderboardMessage>, ICodecProvider<RequestNeighborhoodObjectLeaderboardMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestNeighborhoodObjectLeaderboardMessage> Codec { get; } = Codecs.Composite(
        Codecs.OptionalLongId,
        static message => message.NeighborhoodId,
        static (neighborhoodId) => new RequestNeighborhoodObjectLeaderboardMessage(neighborhoodId)
    );
}
