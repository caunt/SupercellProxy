using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends.Book;

/// <summary>Accepts or declines a friend item request.</summary>
/// <param name="AvatarId">Player who made the request.</param>
/// <param name="Accepted">True to agree to the request; false to turn it down.</param>
public sealed record RespondToFriendRequestMessage(LongId AvatarId, bool Accepted) : CodecMessage<RespondToFriendRequestMessage>, ICodecProvider<RespondToFriendRequestMessage>
{
    /// <inheritdoc />
    public static ICodec<RespondToFriendRequestMessage> Codec { get; } = Codecs.Composite(
        Codecs.LongId,
        static message => message.AvatarId,
        Codecs.Boolean,
        static message => message.Accepted,
        static (avatarId, accepted) => new RespondToFriendRequestMessage(avatarId, accepted)
    );
}
