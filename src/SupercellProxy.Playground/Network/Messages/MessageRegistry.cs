using SupercellProxy.Playground.Network.Messages.Clientbound;
using SupercellProxy.Playground.Network.Messages.Serverbound;

namespace SupercellProxy.Playground.Network.Messages;

public static class MessageRegistry
{
    private record Entry(Type Type, Func<MessageContainer, IMessage> Factory);

    private static readonly Dictionary<ushort, Entry> _map = new()
    {
        [10100] = new Entry(typeof(ClientHelloMessage), ClientHelloMessage.Create),
        [20100] = new Entry(typeof(ServerHelloMessage), ServerHelloMessage.Create),
        [10101] = new Entry(typeof(LoginMessage), LoginMessage.Create),
        [20103] = new Entry(typeof(LoginFailedMessage), LoginFailedMessage.Create),
        [25220] = new Entry(typeof(LoginOkMessage), LoginOkMessage.Create)
    };

    public static IMessage Resolve(MessageContainer container)
    {
        if (!_map.TryGetValue(container.Id, out var entry))
            throw new InvalidOperationException($"Unknown message ID: {container.Id}");

        return entry.Factory(container);
    }

    public static ushort GetId<T>() where T : IMessage
    {
        var type = typeof(T);
        var entry = _map.Values.FirstOrDefault(entry => entry.Type == type)
            ?? throw new InvalidOperationException($"Message type {type} is not registered.");

        return _map.First(kv => kv.Value == entry).Key;
    }
}
