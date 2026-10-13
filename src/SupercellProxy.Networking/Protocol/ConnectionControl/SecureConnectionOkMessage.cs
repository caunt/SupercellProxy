using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.ConnectionControl;

/// <summary>Confirms the secure connection.</summary>
/// <param name="ServerVersion">The server version.</param>
/// <param name="Nonce">The nonce.</param>
public sealed record SecureConnectionOkMessage(string? ServerVersion, string? Nonce) : CodecMessage<SecureConnectionOkMessage>, ICodecProvider<SecureConnectionOkMessage>
{
    /// <inheritdoc />
    public static ICodec<SecureConnectionOkMessage> Codec { get; } = Codecs.Composite(
        Codecs.OptionalString,
        static message => message.ServerVersion,
        Codecs.OptionalString,
        static message => message.Nonce,
        static (serverVersion, nonce) => new SecureConnectionOkMessage(serverVersion, nonce)
    );
}
