using SupercellProxy.Playground.Commands;
using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Messages.Clientbound;
using SupercellProxy.Playground.Network.Messages.Serverbound;

namespace SupercellProxy.Playground.Network.Messages;

/// <summary>
/// Represents <c>MessageRegistry</c>.
/// </summary>
public static class MessageRegistry
{
    private static readonly Dictionary<ushort, string> _hints = new()
    {
        [10518] = "open friend book",
        [14972] = "last helpers request",
        [20155] = "???",
        [20699] = "BaseHomeDataMessage",
        [21628] = "last helpers response",
        [24180] = "OWN_HOME_DATA",
        [26199] = "LogicArrayList<FriendMeta *>",
        [40000] = "updateConversionValue",
    };

    private static readonly Dictionary<ushort, MessageRegistryEntry> _map = new()
    {
        [10100] = new MessageRegistryEntry(
            Version: 0,
            Type: typeof(ClientHelloMessage),
            Factory: ClientHelloMessage.Create
        ),

        [10101] = new MessageRegistryEntry(
            Version: 5213,
            Type: typeof(LoginMessage),
            Factory: LoginMessage.Create
        ),

        [10108] = new MessageRegistryEntry(
            Version: 0,
            Type: typeof(KeepAliveMessage),
            Factory: KeepAliveMessage.Create
        ),

        [14484] = new MessageRegistryEntry(
            Version: 5213,
            Type: typeof(VisitHomeMessage),
            Factory: VisitHomeMessage.Create
        ),

        [17703] = new MessageRegistryEntry(
            Version: 0,
            Type: typeof(VisitOtherFishingHomeMessage),
            Factory: VisitOtherFishingHomeMessage.Create
        ),

        [18671] = new MessageRegistryEntry(
            Version: 5213,
            Type: typeof(VisitHomeTargetMessage),
            Factory: VisitHomeTargetMessage.Create
        ),

        [10224] = new MessageRegistryEntry(
            Version: EndClientTurnMessage.CurrentVersion,
            Type: typeof(EndClientTurnMessage),
            Factory: EndClientTurnMessage.Create
        ),

        [19949] = new MessageRegistryEntry(
            Version: 0,
            Type: typeof(ClientLoadingFunnelMessage),
            Factory: ClientLoadingFunnelMessage.Create
        ),

        [20100] = new MessageRegistryEntry(
            Version: 0,
            Type: typeof(ServerHelloMessage),
            Factory: ServerHelloMessage.Create
        ),

        [20103] = new MessageRegistryEntry(
            Version: 2,
            Type: typeof(LoginFailedMessage),
            Factory: LoginFailedMessage.Create
        ),

        [20108] = new MessageRegistryEntry(
            Version: 0,
            Type: typeof(KeepAliveOkMessage),
            Factory: KeepAliveOkMessage.Create
        ),

        [20187] = new MessageRegistryEntry(
            Version: 0,
            Type: typeof(AvailableServerCommandMessage),
            Factory: AvailableServerCommandMessage.Create
        ),

        [23626] = new MessageRegistryEntry(
            Version: 0,
            Type: typeof(OutOfSyncMessage),
            Factory: OutOfSyncMessage.Create
        ),

        [24180] = new MessageRegistryEntry(
            Version: 0,
            Type: typeof(OwnHomeDataMessage),
            Factory: OwnHomeDataMessage.Create
        ),

        [24489] = new MessageRegistryEntry(
            Version: 0,
            Type: typeof(OtherHomeDataMessage),
            Factory: OtherHomeDataMessage.Create
        ),

        [25220] = new MessageRegistryEntry(
            Version: 2,
            Type: typeof(LoginOkMessage),
            Factory: LoginOkMessage.Create
        ),

        [28917] = new MessageRegistryEntry(
            Version: 0,
            Type: typeof(OtherFishingHomeDataMessage),
            Factory: OtherFishingHomeDataMessage.Create
        ),
    };

    /// <summary>
    /// Resolves <c>MessageRegistry</c> from retained game data.
    /// </summary>
    public static IMessage Resolve(MessageContainer container)
    {
        return Resolve(container, dataResolver: null);
    }

    /// <summary>
    /// Resolves <c>MessageRegistry</c> from retained game data.
    /// </summary>
    public static IMessage Resolve(MessageContainer container, ICommandDataResolver? dataResolver)
    {
        if (!_map.TryGetValue(container.Id, out var entry))
            return PassthroughMessage.Create(container);

        if (container.Id == GetId<EndClientTurnMessage>())
            return EndClientTurnMessage.Create(
                container,
                CommandEnvironment.Production,
                dataResolver
            );

        return entry.Factory(container);
    }

    /// <summary>
    /// Gets <c>Hint</c>.
    /// </summary>
    public static string? GetHint(ushort id)
    {
        return _hints.TryGetValue(id, out var hint) ? hint : null;
    }

    /// <summary>
    /// Gets <c>Id</c>.
    /// </summary>
    public static ushort GetId<T>(T message)
        where T : IMessage
    {
        if (message is PassthroughMessage passthroughMessage)
            return passthroughMessage.Id;

        return GetId(message.GetType());
    }

    /// <summary>
    /// Gets <c>Id</c>.
    /// </summary>
    public static ushort GetId<T>()
        where T : IMessage
    {
        return GetId(typeof(T));
    }

    /// <summary>
    /// Gets <c>Id</c>.
    /// </summary>
    public static ushort GetId(Type type)
    {
        var entry = GetEntry(type);
        return _map.First(kv => kv.Value == entry).Key;
    }

    /// <summary>
    /// Gets <c>Version</c>.
    /// </summary>
    public static ushort GetVersion<T>(T message)
        where T : IMessage
    {
        if (message is PassthroughMessage passthroughMessage)
            return passthroughMessage.Version;

        return GetVersion(message.GetType());
    }

    /// <summary>
    /// Gets <c>Version</c>.
    /// </summary>
    public static ushort GetVersion<T>()
        where T : IMessage
    {
        return GetVersion(typeof(T));
    }

    /// <summary>
    /// Gets <c>Version</c>.
    /// </summary>
    public static ushort GetVersion(Type type)
    {
        return GetEntry(type).Version;
    }

    private static MessageRegistryEntry GetEntry<T>()
        where T : IMessage
    {
        return GetEntry(typeof(T));
    }

    private static MessageRegistryEntry GetEntry(Type type)
    {
        return _map.Values.FirstOrDefault(entry => entry.Type == type)
            ?? throw new InvalidOperationException($"Message type {type} is not registered.");
    }
}
