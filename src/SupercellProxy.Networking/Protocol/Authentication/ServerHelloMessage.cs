using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Authentication;

/// <summary>
/// Represents the <c language="csharp">ServerHelloMessage</c> protocol message.
/// </summary>
public sealed record ServerHelloMessage : CodecMessage<ServerHelloMessage>, ICodecProvider<ServerHelloMessage>
{
    /// <inheritdoc />
    public static ICodec<ServerHelloMessage> Codec { get; } = Codecs.Composite(
        Codecs.ByteArray,
        static message => message.SessionKey,
        static (sessionKey) => new ServerHelloMessage { SessionKey = sessionKey }
    );

    /// <summary>
    /// Gets or sets the <c language="csharp">SessionKey</c> value.
    /// </summary>
    public required ReadOnlyMemory<byte> SessionKey { get; init; }

    /// <summary>
    /// Executes the <c language="csharp">ToString</c> operation.
    /// </summary>
    public override string ToString()
    {
        return $"{nameof(ServerHelloMessage)} {{ {nameof(SessionKey)} = {Convert.ToHexString(SessionKey.Span)} }}";
    }
}
