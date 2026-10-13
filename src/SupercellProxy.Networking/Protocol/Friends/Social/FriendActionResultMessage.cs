using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends.Social;

/// <summary>Reports a friend action result to the social manager's listeners.</summary>
/// <param name="AvatarId">The avatar id.</param>
/// <param name="Result">The result.</param>
public sealed record FriendActionResultMessage(LongId AvatarId, int Result) : CodecMessage<FriendActionResultMessage>, ICodecProvider<FriendActionResultMessage>
{
    /// <inheritdoc />
    public static ICodec<FriendActionResultMessage> Codec { get; } = Codecs.Composite(
        Codecs.LongId,
        static message => message.AvatarId,
        Codecs.VarInt,
        static message => message.Result,
        static (avatarId, result) => new FriendActionResultMessage(avatarId, result)
    );
}
