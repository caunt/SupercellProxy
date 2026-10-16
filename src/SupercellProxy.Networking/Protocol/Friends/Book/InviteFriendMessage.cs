using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends.Book;

/// <summary>Invites a player.</summary>
/// <param name="AvatarId">Player to invite.</param>
/// <param name="Message">Text sent with the invitation.</param>
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
