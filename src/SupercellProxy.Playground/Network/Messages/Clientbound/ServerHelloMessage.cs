using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Network.Messages.Clientbound;

public record ServerHelloMessage : IMessage
{
    public required Memory<byte> SessionKey { get; init; }

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
