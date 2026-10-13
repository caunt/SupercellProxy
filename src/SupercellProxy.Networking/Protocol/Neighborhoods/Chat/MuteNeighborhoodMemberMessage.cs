using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Chat;

/// <summary>Mutes or unmutes a neighborhood member.</summary>
/// <param name="Muted">The muted.</param>
/// <param name="AvatarId">The avatar id.</param>
public sealed record MuteNeighborhoodMemberMessage(bool Muted, LongId AvatarId) : CodecMessage<MuteNeighborhoodMemberMessage>, ICodecProvider<MuteNeighborhoodMemberMessage>
{
    /// <inheritdoc />
    public static ICodec<MuteNeighborhoodMemberMessage> Codec { get; } = Codecs.Composite(
        Codecs.Boolean,
        static message => message.Muted,
        Codecs.LongId,
        static message => message.AvatarId,
        static (muted, avatarId) => new MuteNeighborhoodMemberMessage(muted, avatarId)
    );
}
