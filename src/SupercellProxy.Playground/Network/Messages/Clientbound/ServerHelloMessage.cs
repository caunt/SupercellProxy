using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Network.Messages.Clientbound;

public record ServerHelloMessage : IMessage
{
    public static ushort Id => 20100;

    public required Memory<byte> SessionKey { get; init; }

    static IMessage IMessage.Create(MessageContainer container)
    {
        return Create(container);
    }

    public static ServerHelloMessage Create(MessageContainer container)
    {
        return new ServerHelloMessage
        {
            SessionKey = container.Payload.ReadByteArray()
        };
    }

    public MessageContainer ToContainer(ushort id, ushort version = 0)
    {
        using var supercellStream = SupercellStream.Create();

        supercellStream.WriteByteArray(SessionKey.Span);

        return new MessageContainer(id, version, supercellStream);
    }
}
