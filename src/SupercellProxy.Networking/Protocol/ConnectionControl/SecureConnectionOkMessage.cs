using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.ConnectionControl;

/// <summary>Confirms the secure connection.</summary>
/// <param name="ServerVersion">Version text of the server that accepted the connection.</param>
/// <param name="Nonce">Nonce text the server sends with its confirmation.</param>
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
