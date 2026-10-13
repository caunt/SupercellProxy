using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars.Data;

/// <summary>Requests the avatar stream of a player.</summary>
/// <param name="AvatarId">The avatar id.</param>
public sealed record RequestAvatarStreamMessage(LongId AvatarId) : CodecMessage<RequestAvatarStreamMessage>, ICodecProvider<RequestAvatarStreamMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestAvatarStreamMessage> Codec { get; } = Codecs.Composite(Codecs.LongId, static message => message.AvatarId, static (avatarId) => new RequestAvatarStreamMessage(avatarId));
}
