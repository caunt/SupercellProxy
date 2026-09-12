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
    public static ServerHelloMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);

        return new ServerHelloMessage { SessionKey = container.Payload.ReadByteArray() };
    }

    /// <summary>
    /// Executes the <c language="csharp">ToContainer</c> operation.
    /// </summary>
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream supercellStream = MessageStream.Create();

        supercellStream.WriteByteArray(SessionKey.Span);

        return new MessageContainer(identifier, version, supercellStream);
    }

    /// <summary>
    /// Executes the <c language="csharp">ToString</c> operation.
    /// </summary>
    public override string ToString()
    {
        return $"{nameof(ServerHelloMessage)} {{ {nameof(SessionKey)} = {Convert.ToHexString(SessionKey.Span)} }}";
    }
}
