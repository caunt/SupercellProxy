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
    /// Gets or sets the random bytes the server picked for this connection; the client returns them in the encrypted login so the server can tie the
    /// login to this handshake.
    /// </summary>
    /// <value>
    /// The key as sent, behind a 32-bit big-endian length; the managed client passes it to the stream's encryption setup before writing the login.
    /// </value>
    public required ReadOnlyMemory<byte> SessionKey { get; init; }

    /// <summary>
    /// Shows the session key in hex; it is not secret on its own, since the hello travels unencrypted.
    /// </summary>
    /// <returns>
    /// The type name and the key as uppercase hexadecimal, such as <c>ServerHelloMessage { SessionKey = 0A1B… }</c>; an empty key prints as an
    /// empty string.
    /// </returns>
    public override string ToString()
    {
        return $"{nameof(ServerHelloMessage)} {{ {nameof(SessionKey)} = {Convert.ToHexString(SessionKey.Span)} }}";
    }
}
