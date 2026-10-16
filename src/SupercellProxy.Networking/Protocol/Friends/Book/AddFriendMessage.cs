using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends.Book;

/// <summary>Adds a player found in the friend search.</summary>
/// <param name="AvatarId">Player to send a friend request to.</param>
/// <param name="FromSearch">Whether the player was found through the friend search.</param>
public sealed record AddFriendMessage(LongId AvatarId, bool FromSearch) : CodecMessage<AddFriendMessage>, ICodecProvider<AddFriendMessage>
{
    /// <inheritdoc />
    public static ICodec<AddFriendMessage> Codec { get; } = Codecs.Composite(
        Codecs.LongId,
        static message => message.AvatarId,
        Codecs.Boolean,
        static message => message.FromSearch,
        static (avatarId, fromSearch) => new AddFriendMessage(avatarId, fromSearch)
    );
}
