using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars.Data;

/// <summary>Requests the avatar of another player.</summary>
/// <param name="AvatarId">The avatar id.</param>
public sealed record RequestOtherAvatarMessage(LongId AvatarId) : CodecMessage<RequestOtherAvatarMessage>, ICodecProvider<RequestOtherAvatarMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestOtherAvatarMessage> Codec { get; } = Codecs.Composite(Codecs.LongId, static message => message.AvatarId, static (avatarId) => new RequestOtherAvatarMessage(avatarId));
}
