using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Chat;

/// <summary>Answers a member mute; the neighborhood manager notifies its listeners.</summary>
/// <param name="Muted">The muted.</param>
/// <param name="AvatarId">The avatar id.</param>
/// <param name="NeighborhoodId">The neighborhood id.</param>
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
