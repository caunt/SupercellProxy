using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars.Information;

/// <summary>Answers AvatarInformationListRequestMessage with each avatar's social ids and farm name (native AvatarInfoListResponseMessage).</summary>
/// <param name="Avatars">The avatars.</param>
public sealed record AvatarInformationListResponseMessage(ReadOnlyMemory<AvatarInformation>? Avatars) : CodecMessage<AvatarInformationListResponseMessage>, ICodecProvider<AvatarInformationListResponseMessage>
{
    /// <inheritdoc />
    public static ICodec<AvatarInformationListResponseMessage> Codec { get; } = Codecs.Composite(
        Codecs.NullableMemory(AvatarInformation.Codec),
        static message => message.Avatars,
        static avatars => new AvatarInformationListResponseMessage(avatars)
    );
}
