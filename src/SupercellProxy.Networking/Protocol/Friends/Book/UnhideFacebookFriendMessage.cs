using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends.Book;

/// <summary>Unhides a Facebook friend.</summary>
/// <param name="AvatarId">The avatar id.</param>
public sealed record UnhideFacebookFriendMessage(LongId AvatarId) : CodecMessage<UnhideFacebookFriendMessage>, ICodecProvider<UnhideFacebookFriendMessage>
{
    /// <inheritdoc />
    public static ICodec<UnhideFacebookFriendMessage> Codec { get; } = Codecs.Composite(Codecs.LongId, static message => message.AvatarId, static (avatarId) => new UnhideFacebookFriendMessage(avatarId));
}
