using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Authentication;

/// <summary>
/// Clientbound answer to the client's hello, sent unencrypted. It hands over the session key the client must echo inside its
/// encrypted login, after which both sides switch to encrypted messages.
/// </summary>
public sealed record ServerHelloMessage : CodecMessage<ServerHelloMessage>, ICodecProvider<ServerHelloMessage>
{
    /// <inheritdoc />
    public static ICodec<ServerHelloMessage> Codec { get; } = Codecs.Composite(Codecs.ByteArray, static message => message.SessionKey, static sessionKey => new ServerHelloMessage { SessionKey = sessionKey });

    /// <summary>
    /// Random bytes the server picked for this connection; the client returns them in the encrypted login so the server can tie the
    /// login to this handshake.
    /// </summary>
    public required ReadOnlyMemory<byte> SessionKey { get; init; }

    /// <summary>
    /// Shows the session key in hex; it is not secret on its own, since the hello travels unencrypted.
    /// </summary>
    public override string ToString()
    {
        return $"{nameof(ServerHelloMessage)} {{ {nameof(SessionKey)} = {Convert.ToHexString(SessionKey.Span)} }}";
    }
}
