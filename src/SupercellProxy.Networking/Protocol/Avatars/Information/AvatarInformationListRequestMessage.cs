using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars.Information;

/// <summary>Requests the public info of several avatars.</summary>
/// <param name="AvatarIds">The avatar ids.</param>
public sealed record AvatarInformationListRequestMessage(ReadOnlyMemory<LongId>? AvatarIds) : CodecMessage<AvatarInformationListRequestMessage>, ICodecProvider<AvatarInformationListRequestMessage>
{
    /// <inheritdoc />
    public static ICodec<AvatarInformationListRequestMessage> Codec { get; } = Codecs.Composite(
        Codecs.NullableMemory(Codecs.LongId),
        static message => message.AvatarIds,
        static avatarIds => new AvatarInformationListRequestMessage(avatarIds)
    );
}
