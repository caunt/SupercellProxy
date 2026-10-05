using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Authentication;

/// <summary>
/// Represents the <c language="csharp">ServerHelloMessage</c> protocol message.
/// </summary>
public sealed record ServerHelloMessage : IMessage
{
    /// <summary>
    /// Gets or sets the <c language="csharp">SessionKey</c> value.
    /// </summary>
    public required Memory<byte> SessionKey { get; init; }

    /// <summary>
    /// Creates a <c language="csharp">ServerHelloMessage</c> from the supplied data.
    /// </summary>
    public static ServerHelloMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new ServerHelloMessage { SessionKey = stream.ReadByteArray() };
    }

    /// <summary>
    /// Executes the <c language="csharp">Encode</c> operation.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.WriteByteArray(SessionKey.Span);
    }

    /// <summary>
    /// Executes the <c language="csharp">ToString</c> operation.
    /// </summary>
    public override string ToString()
    {
        return $"{nameof(ServerHelloMessage)} {{ {nameof(SessionKey)} = {Convert.ToHexString(SessionKey.Span)} }}";
    }
}
