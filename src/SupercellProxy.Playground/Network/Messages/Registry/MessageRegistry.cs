using SupercellProxy.Playground.Commands;
using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Messages.Clientbound;
using SupercellProxy.Playground.Network.Messages.Serverbound;

namespace SupercellProxy.Playground.Network.Messages;

/// <summary>
/// Represents <c language="csharp">MessageRegistry</c>.
/// </summary>
internal static class MessageRegistry
{
    /// Identifies the clientbound loading-complete gate used to initialize home turns.
    public const ushort HomeInitializationMessageType = 27439;

    private static readonly Dictionary<ushort, string> Hints = new()
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

    private static readonly Dictionary<ushort, MessageRegistryEntry> Map = new()
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

        [20155] = new MessageRegistryEntry(
            Version: 0,
            Type: typeof(Clientbound20155Message),
            Factory: Clientbound20155Message.Create
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

        [26199] = new MessageRegistryEntry(
            Version: 0,
            Type: typeof(Clientbound26199Message),
            Factory: Clientbound26199Message.Create
        ),

        [28917] = new MessageRegistryEntry(
            Version: 0,
            Type: typeof(OtherFishingHomeDataMessage),
            Factory: OtherFishingHomeDataMessage.Create
        ),
    };

    /// <summary>
    /// Resolves <c language="csharp">MessageRegistry</c> from retained game data.
    /// </summary>
    public static IMessage Resolve(MessageContainer container)
    {
        return Resolve(container, dataResolver: null);
    }

    /// <summary>
    /// Resolves <c language="csharp">MessageRegistry</c> from retained game data.
    /// </summary>
    public static IMessage Resolve(MessageContainer container, ICommandDataResolver? dataResolver)
    {
        if (!Map.TryGetValue(container.Id, out var entry))
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
    /// Gets <c language="csharp">Hint</c>.
    /// </summary>
    public static string? GetHint(ushort id)
    {
        return Hints.TryGetValue(id, out var hint) ? hint : null;
    }

    /// <summary>
    /// Gets <c language="csharp">Id</c>.
    /// </summary>
    public static ushort GetId<T>(T message)
        where T : IMessage
    {
        if (message is PassthroughMessage passthroughMessage)
            return passthroughMessage.Id;

        return GetId(message.GetType());
    }

    /// <summary>
    /// Gets <c language="csharp">Id</c>.
    /// </summary>
    public static ushort GetId<T>()
        where T : IMessage
    {
        return GetId(typeof(T));
    }

    /// <summary>
    /// Gets <c language="csharp">Id</c>.
    /// </summary>
    public static ushort GetId(Type type)
    {
        var entry = GetEntry(type);
        return Map.First(kv => kv.Value == entry).Key;
    }

    /// <summary>
    /// Gets <c language="csharp">Version</c>.
    /// </summary>
    public static ushort GetVersion<T>(T message)
        where T : IMessage
    {
        if (message is PassthroughMessage passthroughMessage)
            return passthroughMessage.Version;

        return GetVersion(message.GetType());
    }

    /// <summary>
    /// Gets <c language="csharp">Version</c>.
    /// </summary>
    public static ushort GetVersion<T>()
        where T : IMessage
    {
        return GetVersion(typeof(T));
    }

    /// <summary>
    /// Gets <c language="csharp">Version</c>.
    /// </summary>
    public static ushort GetVersion(Type type)
    {
        return GetEntry(type).Version;
    }

    private static MessageRegistryEntry GetEntry(Type type)
    {
        return Map.Values.FirstOrDefault(entry => entry.Type == type)
            ?? throw new InvalidOperationException($"Message type {type} is not registered.");
    }
}
