using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends.Book;

/// <summary>Hides a Facebook friend.</summary>
/// <param name="AvatarId">The avatar id.</param>
public sealed record HideFacebookFriendMessage(LongId AvatarId) : CodecMessage<HideFacebookFriendMessage>, ICodecProvider<HideFacebookFriendMessage>
{
    /// <inheritdoc />
    public static ICodec<HideFacebookFriendMessage> Codec { get; } = Codecs.Composite(Codecs.LongId, static message => message.AvatarId, static (avatarId) => new HideFacebookFriendMessage(avatarId));
}
