using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Network.Messages.Clientbound;

public record KeepAliveOkMessage : IMessage
{
    public static KeepAliveOkMessage Create(MessageContainer container)
    {
        return new KeepAliveOkMessage();
    }

    public MessageContainer ToContainer(ushort id, ushort version = 5209)
    {
        using var supercellStream = SupercellStream.Create();

        return new MessageContainer(id, version, supercellStream);
    }
}
