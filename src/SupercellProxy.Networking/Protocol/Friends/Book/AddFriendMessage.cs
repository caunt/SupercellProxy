using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends.Book;

/// <summary>Adds a player found in the friend search.</summary>
/// <param name="AvatarId">The avatar id.</param>
/// <param name="FromSearch">The from search.</param>
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
