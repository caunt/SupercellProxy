using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Objects;

/// <summary>Requests the rewards of a neighborhood object.</summary>
/// <param name="NeighborhoodId">Neighbourhood that owns the object.</param>
/// <param name="ObjectId">Id of the neighbourhood event object whose rewards to show.</param>
public sealed record RequestNeighborhoodObjectRewardsMessage(LongId? NeighborhoodId, int ObjectId) : CodecMessage<RequestNeighborhoodObjectRewardsMessage>, ICodecProvider<RequestNeighborhoodObjectRewardsMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestNeighborhoodObjectRewardsMessage> Codec { get; } = Codecs.Composite(
        Codecs.OptionalLongId,
        static message => message.NeighborhoodId,
        Codecs.VarInt,
        static message => message.ObjectId,
        static (neighborhoodId, objectId) => new RequestNeighborhoodObjectRewardsMessage(neighborhoodId, objectId)
    );
}
