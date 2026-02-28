namespace SupercellProxy.Playground.Network.Messages;

public interface IMessage
{
    public static abstract ushort Id { get; }

    public static abstract IMessage Create(MessageContainer container);
    public MessageContainer ToContainer(ushort id, ushort version = 0);
}
