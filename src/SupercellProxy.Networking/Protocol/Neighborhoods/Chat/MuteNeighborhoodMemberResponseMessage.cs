using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Chat;

/// <summary>Answers a member mute; the neighborhood manager notifies its listeners.</summary>
/// <param name="Muted">Whether the member is now muted.</param>
/// <param name="AvatarId">Account of the member.</param>
/// <param name="NeighborhoodId">Neighbourhood the member belongs to.</param>
public sealed record MuteNeighborhoodMemberResponseMessage(bool Muted, LongId AvatarId, LongId NeighborhoodId) : CodecMessage<MuteNeighborhoodMemberResponseMessage>, ICodecProvider<MuteNeighborhoodMemberResponseMessage>
{
    /// <inheritdoc />
    public static ICodec<MuteNeighborhoodMemberResponseMessage> Codec { get; } = Codecs.Composite(
        Codecs.Boolean,
        static message => message.Muted,
        Codecs.LongId,
        static message => message.AvatarId,
        Codecs.LongId,
        static message => message.NeighborhoodId,
        static (muted, avatarId, neighborhoodId) => new MuteNeighborhoodMemberResponseMessage(muted, avatarId, neighborhoodId)
    );
}
