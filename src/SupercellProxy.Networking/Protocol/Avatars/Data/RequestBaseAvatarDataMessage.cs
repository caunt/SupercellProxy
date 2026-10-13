using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars.Data;

/// <summary>Requests the avatar's base data.</summary>
public sealed record RequestBaseAvatarDataMessage : CodecMessage<RequestBaseAvatarDataMessage>, ICodecProvider<RequestBaseAvatarDataMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestBaseAvatarDataMessage> Codec { get; } = Codecs.Unit(static () => new RequestBaseAvatarDataMessage());
}
