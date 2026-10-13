using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends.Book;

/// <summary>Invites a player.</summary>
/// <param name="AvatarId">The avatar id.</param>
/// <param name="Message">The message.</param>
public sealed record InviteFriendMessage(LongId AvatarId, string? Message) : CodecMessage<InviteFriendMessage>, ICodecProvider<InviteFriendMessage>
{
    /// <inheritdoc />
    public static ICodec<InviteFriendMessage> Codec { get; } = Codecs.Composite(
        Codecs.LongId,
        static message => message.AvatarId,
        Codecs.OptionalString,
        static message => message.Message,
        static (avatarId, message) => new InviteFriendMessage(avatarId, message)
    );
}
