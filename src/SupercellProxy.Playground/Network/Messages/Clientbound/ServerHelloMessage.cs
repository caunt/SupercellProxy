using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Network.Messages.Clientbound;

/// <summary>
/// Represents the <c language="csharp">ServerHelloMessage</c> protocol message.
/// </summary>
internal sealed record ServerHelloMessage : IMessage
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
        return new ServerHelloMessage { SessionKey = container.Payload.ReadByteArray() };
    }

    /// <summary>
    /// Executes the <c language="csharp">ToContainer</c> operation.
    /// </summary>
    public MessageContainer ToContainer(ushort id, ushort version = 0)
    {
        using var supercellStream = MessageStream.Create();

        supercellStream.WriteByteArray(SessionKey.Span);

        return new MessageContainer(id, version, supercellStream);
    }

    /// <summary>
    /// Executes the <c language="csharp">ToString</c> operation.
    /// </summary>
    public override string ToString()
    {
        return $"{nameof(ServerHelloMessage)} {{ {nameof(SessionKey)} = {Convert.ToHexString(SessionKey.Span)} }}";
    }
}
