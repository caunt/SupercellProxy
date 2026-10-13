using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends.Book;

/// <summary>Accepts a pending friend request.</summary>
/// <param name="AvatarId">The avatar id.</param>
public sealed record AcceptFriendRequestMessage(LongId AvatarId) : CodecMessage<AcceptFriendRequestMessage>, ICodecProvider<AcceptFriendRequestMessage>
{
    /// <inheritdoc />
    public static ICodec<AcceptFriendRequestMessage> Codec { get; } = Codecs.Composite(Codecs.LongId, static message => message.AvatarId, static (avatarId) => new AcceptFriendRequestMessage(avatarId));
}
