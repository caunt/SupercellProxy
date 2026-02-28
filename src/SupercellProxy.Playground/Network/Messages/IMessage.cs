namespace SupercellProxy.Playground.Network.Messages;

public interface IMessage
{
    public MessageContainer ToContainer(ushort id, ushort version = 0);
}
