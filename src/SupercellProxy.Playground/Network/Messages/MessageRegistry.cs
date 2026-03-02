using SupercellProxy.Playground.Network.Messages.Clientbound;
using SupercellProxy.Playground.Network.Messages.Serverbound;

namespace SupercellProxy.Playground.Network.Messages;

public static class MessageRegistry
{
    private record Entry(ushort Version, Type Type, Func<MessageContainer, IMessage> Factory);

    private static readonly Dictionary<ushort, string> _hints = new()
    {
        [10518] = "open friend book",
        [14972] = "last helpers request",
        [20155] = "???",
        [20699] = "BaseHomeDataMessage",
        [21628] = "last helpers response",
        [24180] = "OWN_HOME_DATA",
        [26199] = "LogicArrayList<FriendMeta *>",
        [40000] = "updateConversionValue"
    };

    private static readonly Dictionary<ushort, Entry> _map = new()
    {
        [10100] = new Entry(
            Version: 0,
            Type: typeof(ClientHelloMessage),
            Factory: ClientHelloMessage.Create),

        [10101] = new Entry(
            Version: 5209,
            Type: typeof(LoginMessage),
            Factory: LoginMessage.Create),

        [10108] = new Entry(
            Version: 0,
            Type: typeof(KeepAliveMessage),
            Factory: KeepAliveMessage.Create),

        [14484] = new Entry(
            Version: 5213,
            Type: typeof(VisitHomeMessage),
            Factory: VisitHomeMessage.Create),

        [18671] = new Entry(
            Version: 5213,
            Type: typeof(VisitHomeTargetMessage),
            Factory: VisitHomeTargetMessage.Create),

        [19132] = new Entry(
            Version: 5213,
            Type: typeof(EndClientTurnMessage),
            Factory: EndClientTurnMessage.Create),

        [20100] = new Entry(
            Version: 0,
            Type: typeof(ServerHelloMessage),
            Factory: ServerHelloMessage.Create),

        [20103] = new Entry(
            Version: 2,
            Type: typeof(LoginFailedMessage),
            Factory: LoginFailedMessage.Create),

        [20108] = new Entry(
            Version: 0,
            Type: typeof(KeepAliveOkMessage),
            Factory: KeepAliveOkMessage.Create),

        [24489] = new Entry(
            Version: 0,
            Type: typeof(OtherHomeDataMessage),
            Factory: OtherHomeDataMessage.Create),

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

    public static string? GetHint(ushort id)
    {
        return _hints.TryGetValue(id, out var hint) ? hint : null;
    }

    public static ushort GetId<T>(T message) where T : IMessage
    {
        if (message is PassthroughMessage passthroughMessage)
            return passthroughMessage.Id;

        return GetId(message.GetType());
    }

    public static ushort GetId<T>() where T : IMessage
    {
        return GetId(typeof(T));
    }

    public static ushort GetId(Type type)
    {
        var entry = GetEntry(type);
        return _map.First(kv => kv.Value == entry).Key;
    }

    public static ushort GetVersion<T>(T message) where T : IMessage
    {
        if (message is PassthroughMessage passthroughMessage)
            return passthroughMessage.Version;

        return GetVersion(message.GetType());
    }

    public static ushort GetVersion<T>() where T : IMessage
    {
        return GetVersion(typeof(T));
    }

    public static ushort GetVersion(Type type)
    {
        return GetEntry(type).Version;
    }

    private static Entry GetEntry<T>() where T : IMessage
    {
        return GetEntry(typeof(T));
    }

    private static Entry GetEntry(Type type)
    {
        return _map.Values.FirstOrDefault(entry => entry.Type == type) ?? throw new InvalidOperationException($"Message type {type} is not registered.");
    }
}
