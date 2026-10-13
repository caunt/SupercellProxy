using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Membership;

/// <summary>Kicks a member from a neighborhood.</summary>
/// <param name="NeighborhoodId">The neighborhood id.</param>
/// <param name="AvatarId">The avatar id.</param>
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
