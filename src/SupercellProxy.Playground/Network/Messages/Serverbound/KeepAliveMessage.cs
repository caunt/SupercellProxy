using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Network.Messages.Serverbound;

public record KeepAliveMessage : IMessage
{
    public static KeepAliveMessage Create(MessageContainer container)
    {
        return new KeepAliveMessage();
    }

    public MessageContainer ToContainer(ushort id, ushort version = 0)
    {
        using var supercellStream = SupercellStream.Create();

        return new MessageContainer(id, version, supercellStream);
    }
}
