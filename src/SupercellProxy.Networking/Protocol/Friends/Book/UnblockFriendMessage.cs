using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends.Book;

/// <summary>Unblocks a blocked player in the friend book.</summary>
/// <param name="AvatarId">Player to unblock.</param>
public sealed record UnblockFriendMessage(LongId AvatarId) : CodecMessage<UnblockFriendMessage>, ICodecProvider<UnblockFriendMessage>
{
    /// <inheritdoc />
    public static ICodec<UnblockFriendMessage> Codec { get; } = Codecs.Composite(Codecs.LongId, static message => message.AvatarId, static avatarId => new UnblockFriendMessage(avatarId));
}
