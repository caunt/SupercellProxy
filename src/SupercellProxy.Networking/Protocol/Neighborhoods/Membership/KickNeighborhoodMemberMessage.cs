using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Membership;

/// <summary>Kicks a member from a neighborhood.</summary>
/// <param name="NeighborhoodId">Neighbourhood the member is removed from.</param>
/// <param name="AvatarId">Account of the member to remove.</param>
public sealed record KickNeighborhoodMemberMessage(LongId NeighborhoodId, LongId AvatarId) : CodecMessage<KickNeighborhoodMemberMessage>, ICodecProvider<KickNeighborhoodMemberMessage>
{
    /// <inheritdoc />
    public static ICodec<KickNeighborhoodMemberMessage> Codec { get; } = Codecs.Composite(
        Codecs.LongId,
        static message => message.NeighborhoodId,
        Codecs.LongId,
        static message => message.AvatarId,
        static (neighborhoodId, avatarId) => new KickNeighborhoodMemberMessage(neighborhoodId, avatarId)
    );
}
