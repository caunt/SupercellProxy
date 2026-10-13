using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends.Book;

/// <summary>Unfollows a player or removes a follower.</summary>
/// <param name="AvatarId">The avatar id.</param>
public sealed record UnfollowFriendMessage(LongId AvatarId) : CodecMessage<UnfollowFriendMessage>, ICodecProvider<UnfollowFriendMessage>
{
    /// <inheritdoc />
    public static ICodec<UnfollowFriendMessage> Codec { get; } = Codecs.Composite(Codecs.LongId, static message => message.AvatarId, static (avatarId) => new UnfollowFriendMessage(avatarId));
}
