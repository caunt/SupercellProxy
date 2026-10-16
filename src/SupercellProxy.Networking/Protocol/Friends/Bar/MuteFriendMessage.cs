using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends.Bar;

/// <summary>Mutes a player shown in the friend bar.</summary>
/// <param name="AvatarId">Player to mute.</param>
public sealed record MuteFriendMessage(LongId AvatarId) : CodecMessage<MuteFriendMessage>, ICodecProvider<MuteFriendMessage>
{
    /// <inheritdoc />
    public static ICodec<MuteFriendMessage> Codec { get; } = Codecs.Composite(Codecs.LongId, static message => message.AvatarId, static avatarId => new MuteFriendMessage(avatarId));
}
