using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends.Social;

/// <summary>Reports a friend request result to the social manager's listeners.</summary>
/// <param name="AvatarId">Player the friend request was sent to.</param>
/// <param name="Result">Outcome code of the request, as the server numbers them.</param>
public sealed record FriendRequestResultMessage(LongId AvatarId, int Result) : CodecMessage<FriendRequestResultMessage>, ICodecProvider<FriendRequestResultMessage>
{
    /// <inheritdoc />
    public static ICodec<FriendRequestResultMessage> Codec { get; } = Codecs.Composite(
        Codecs.LongId,
        static message => message.AvatarId,
        Codecs.VarInt,
        static message => message.Result,
        static (avatarId, result) => new FriendRequestResultMessage(avatarId, result)
    );
}
