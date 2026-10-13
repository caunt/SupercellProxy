using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars.Information;

/// <summary>Requests the profile of an avatar.</summary>
/// <param name="AvatarId">The avatar id.</param>
public sealed record RequestAvatarProfileMessage(LongId AvatarId) : CodecMessage<RequestAvatarProfileMessage>, ICodecProvider<RequestAvatarProfileMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestAvatarProfileMessage> Codec { get; } = Codecs.Composite(Codecs.LongId, static message => message.AvatarId, static (avatarId) => new RequestAvatarProfileMessage(avatarId));
}
