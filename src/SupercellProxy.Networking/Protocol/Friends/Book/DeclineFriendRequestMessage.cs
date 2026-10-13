using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends.Book;

/// <summary>Declines or blocks a pending friend request.</summary>
/// <param name="AvatarId">The avatar id.</param>
public sealed record DeclineFriendRequestMessage(LongId AvatarId) : CodecMessage<DeclineFriendRequestMessage>, ICodecProvider<DeclineFriendRequestMessage>
{
    /// <inheritdoc />
    public static ICodec<DeclineFriendRequestMessage> Codec { get; } = Codecs.Composite(Codecs.LongId, static message => message.AvatarId, static (avatarId) => new DeclineFriendRequestMessage(avatarId));
}
