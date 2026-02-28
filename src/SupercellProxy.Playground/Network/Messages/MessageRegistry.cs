using SupercellProxy.Playground.Network.Messages.Clientbound;
using SupercellProxy.Playground.Network.Messages.Serverbound;

namespace SupercellProxy.Playground.Network.Messages;

public static class MessageRegistry
{
    private record Entry(ushort Version, Type Type, Func<MessageContainer, IMessage> Factory);

    private static readonly Dictionary<ushort, Entry> _map = new()
    {
        [10100] = new Entry(
            Version: 0,
            Type: typeof(ClientHelloMessage),
            Factory: ClientHelloMessage.Create),

        [20100] = new Entry(
            Version: 0,
            Type: typeof(ServerHelloMessage),
            Factory: ServerHelloMessage.Create),

        [10101] = new Entry(
            Version: 5209,
            Type: typeof(LoginMessage),
            Factory: LoginMessage.Create),

        [20103] = new Entry(
            Version: 2,
            Type: typeof(LoginFailedMessage),
            Factory: LoginFailedMessage.Create),

        [25220] = new Entry(
            Version: 2,
            Type: typeof(LoginOkMessage),
            Factory: LoginOkMessage.Create)
    };

    public static IMessage Resolve(MessageContainer container)
    {
        if (!_map.TryGetValue(container.Id, out var entry))
            return PassthroughMessage.Create(container);

        return entry.Factory(container);
    }

    public static ushort GetId<T>(T message) where T : IMessage
    {
        if (message is PassthroughMessage passthroughMessage)
            return passthroughMessage.Id;

        return GetId<T>();
    }

    public static ushort GetId<T>() where T : IMessage
    {
        var entry = GetEntry<T>();
        return _map.First(kv => kv.Value == entry).Key;
    }

    public static ushort GetVersion<T>(T message) where T : IMessage
    {
        if (message is PassthroughMessage passthroughMessage)
            return passthroughMessage.Version;

        return GetVersion<T>();
    }

    public static ushort GetVersion<T>() where T : IMessage
    {
        return GetEntry<T>().Version;
    }

    private static Entry GetEntry<T>()
    {
        var type = typeof(T);
        return _map.Values.FirstOrDefault(entry => entry.Type == type) ?? throw new InvalidOperationException($"Message type {type} is not registered.");
    }
}
