using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars.Data;

/// <summary>Delivers avatar data; native only reads it while bytes remain.</summary>
/// <param name="Data">The avatar data native reads only when bytes remain.</param>
public sealed record AvatarDataMessage(ReadOnlyMemory<byte>? Data) : CodecMessage<AvatarDataMessage>, ICodecProvider<AvatarDataMessage>
{
    /// <inheritdoc />
    /// <remarks>Native reads the data only while payload bytes remain.</remarks>
    public static ICodec<AvatarDataMessage> Codec { get; } = Codecs.Composite(
        Codecs.Trailing(Codecs.OptionalByteArray, absent: null, static data => data is not null),
        static message => message.Data,
        static data => new AvatarDataMessage(data)
    );
}
