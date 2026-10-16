using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Membership;

/// <summary>Requests the member list of a neighborhood.</summary>
/// <param name="NeighborhoodId">Neighbourhood whose members to list.</param>
public sealed record RequestNeighborhoodMembersMessage(LongId? NeighborhoodId) : CodecMessage<RequestNeighborhoodMembersMessage>, ICodecProvider<RequestNeighborhoodMembersMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestNeighborhoodMembersMessage> Codec { get; } = Codecs.Composite(
        Codecs.OptionalLongId,
        static message => message.NeighborhoodId,
        static neighborhoodId => new RequestNeighborhoodMembersMessage(neighborhoodId)
    );
}
