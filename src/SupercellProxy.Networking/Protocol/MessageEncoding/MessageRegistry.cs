using SupercellProxy.Networking.Protocol.Accounts;
using SupercellProxy.Networking.Protocol.Authentication;
using SupercellProxy.Networking.Protocol.CollectionPayloads;
using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.ConnectionControl;
using SupercellProxy.Networking.Protocol.Friends;
using SupercellProxy.Networking.Protocol.Homes;
using SupercellProxy.Networking.Protocol.Neighborhoods;
using SupercellProxy.Networking.Protocol.Newspapers;
using SupercellProxy.Networking.Protocol.OpaquePayloads;
using SupercellProxy.Networking.Protocol.Rankings;
using SupercellProxy.Networking.Protocol.ResourceAssociations;
using SupercellProxy.Networking.Protocol.RoadsideShops;
using SupercellProxy.Networking.Protocol.ScalarPayloads;
using SupercellProxy.Networking.Protocol.Turns;


namespace SupercellProxy.Networking.Protocol.MessageEncoding;

/// <summary>
/// Represents <c language="csharp">MessageRegistry</c>.
/// </summary>
public static class MessageRegistry
{

    /// <summary>
    /// Provides the Clientbound28544 Message Type value or operation.
    /// </summary>
    public const ushort Clientbound28544MessageType = 28544;

    /// Identifies the clientbound loading-complete gate used to initialize home turns.
    public const ushort HomeInitializationMessageType = 27439;

    private static readonly Dictionary<ushort, string> Hints = new()
    {
        [key: 10518] = "open friend book",
        [key: 14972] = "last helpers request",
        [key: 20155] = "???",
        [key: 20699] = "BaseHomeDataMessage",
        [key: 21628] = "last helpers response",
        [key: 24180] = "OWN_HOME_DATA",
        [key: 26199] = "LogicArrayList<FriendMeta *>",
        [key: 40000] = "updateConversionValue",
    };

    private static readonly Dictionary<ushort, MessageRegistryEntry> Map = new()
    {
        [key: 22878] = new MessageRegistryEntry(Version: 0, Type: typeof(RoadsidePurchaseResultMessage), Factory: RoadsidePurchaseResultMessage.Create)
        { CaptureName = "RoadsidePurchaseResultMessage" },
        [key: 28562] = new MessageRegistryEntry(Version: 0, Type: typeof(RoadsideListingBuyerMessage), Factory: RoadsideListingBuyerMessage.Create)
        { CaptureName = "RoadsideListingBuyerMessage" },
        [key: 21767] = new MessageRegistryEntry(Version: 0, Type: typeof(HomeLoadFailedMessage), Factory: HomeLoadFailedMessage.Create)
        { CaptureName = "HomeLoadFailedMessage" },
        [key: 10100] = new MessageRegistryEntry(Version: 0, Type: typeof(ClientHelloMessage), Factory: ClientHelloMessage.Create)
        { CaptureName = "ClientHelloMessage" },

        [key: 10101] = new MessageRegistryEntry(Version: 5213, Type: typeof(LoginMessage), Factory: LoginMessage.Create)
        { CaptureName = "LoginMessage" },

        [key: 10108] = new MessageRegistryEntry(Version: 0, Type: typeof(KeepAliveMessage), Factory: KeepAliveMessage.Create)
        { CaptureName = "KeepAliveMessage" },

        [key: 14484] = new MessageRegistryEntry(Version: 5213, Type: typeof(VisitHomeMessage), Factory: VisitHomeMessage.Create)
        { CaptureName = "VisitHomeMessage" },

        [key: 17703] = new MessageRegistryEntry(Version: 0, Type: typeof(VisitOtherFishingHomeMessage), Factory: VisitOtherFishingHomeMessage.Create)
        { CaptureName = "VisitOtherFishingHomeMessage" },

        [key: 18671] = new MessageRegistryEntry(Version: 5213, Type: typeof(VisitHomeTargetMessage), Factory: VisitHomeTargetMessage.Create)
        { CaptureName = "VisitHomeTargetMessage" },

        [key: 10224] = new MessageRegistryEntry(Version: EndClientTurnMessage.CurrentVersion, Type: typeof(EndClientTurnMessage), Factory: EndClientTurnMessage.Create)
        { CaptureName = "EndClientTurnMessage" },

        [key: 19949] = new MessageRegistryEntry(Version: 0, Type: typeof(RequestOwnHomeMessage), Factory: RequestOwnHomeMessage.Create)
        { CaptureName = "RequestOwnHomeMessage" },

        [key: 20013] = new MessageRegistryEntry(Version: 0, Type: typeof(NeighborhoodListsMessage), Factory: NeighborhoodListsMessage.Create)
        { CaptureName = "NeighborhoodListsMessage" },

        [key: 22158] = new MessageRegistryEntry(Version: 0, Type: typeof(RoadsideBuyerMessage), Factory: RoadsideBuyerMessage.Create)
        { CaptureName = "RoadsideBuyerMessage" },

        [key: 26668] = new MessageRegistryEntry(Version: 0, Type: typeof(HomeVisitStatusMessage), Factory: HomeVisitStatusMessage.Create)
        { CaptureName = "Clientbound26668Message" },

        [key: 20100] = new MessageRegistryEntry(Version: 0, Type: typeof(ServerHelloMessage), Factory: ServerHelloMessage.Create)
        { CaptureName = "ServerHelloMessage" },

        [key: 20103] = new MessageRegistryEntry(Version: 2, Type: typeof(LoginFailedMessage), Factory: LoginFailedMessage.Create)
        { CaptureName = "LoginFailedMessage" },

        [key: 20108] = new MessageRegistryEntry(Version: 0, Type: typeof(KeepAliveOkMessage), Factory: KeepAliveOkMessage.Create)
        { CaptureName = "KeepAliveOkMessage" },

        [key: 20155] = new MessageRegistryEntry(Version: 0, Type: typeof(Clientbound20155Message), Factory: Clientbound20155Message.Create)
        { CaptureName = "Clientbound20155Message" },

        [key: 20187] = new MessageRegistryEntry(Version: 0, Type: typeof(AvailableServerCommandMessage), Factory: AvailableServerCommandMessage.Create)
        { CaptureName = "AvailableServerCommandMessage" },

        [key: 20621] = new MessageRegistryEntry(Version: 0, Type: typeof(Clientbound20621Message), Factory: Clientbound20621Message.Create)
        { CaptureName = "Clientbound20621Message" },

        [key: 21915] = new MessageRegistryEntry(Version: 0, Type: typeof(Clientbound21915Message), Factory: Clientbound21915Message.Create)
        { CaptureName = "Clientbound21915Message" },

        [key: 21945] = new MessageRegistryEntry(Version: 0, Type: typeof(Clientbound21945Message), Factory: Clientbound21945Message.Create)
        { CaptureName = "Clientbound21945Message" },

        [key: 22903] = new MessageRegistryEntry(Version: 0, Type: typeof(Clientbound22903Message), Factory: Clientbound22903Message.Create)
        { CaptureName = "Clientbound22903Message" },
        [key: 28967] = new MessageRegistryEntry(Version: 0, Type: typeof(NewspaperDataMessage), Factory: NewspaperDataMessage.Create)
        { CaptureName = "NewspaperDataMessage" },
        [key: 26994] = new MessageRegistryEntry(Version: 0, Type: typeof(Clientbound26994Message), Factory: Clientbound26994Message.Create)
        { CaptureName = "Clientbound26994Message" },

        [key: 22302] = new MessageRegistryEntry(Version: 0, Type: typeof(Clientbound22302Message), Factory: Clientbound22302Message.Create)
        { CaptureName = "Clientbound22302Message" },

        [key: 22802] = new MessageRegistryEntry(Version: 0, Type: typeof(Clientbound22802Message), Factory: Clientbound22802Message.Create)
        { CaptureName = "Clientbound22802Message" },

        [key: 23074] = new MessageRegistryEntry(Version: 0, Type: typeof(Clientbound23074Message), Factory: Clientbound23074Message.Create)
        { CaptureName = "Clientbound23074Message" },

        [key: 23443] = new MessageRegistryEntry(Version: 0, Type: typeof(PlayerRankingsMessage), Factory: PlayerRankingsMessage.Create)
        { CaptureName = "PlayerRankingsMessage" },

        [key: 23444] = new MessageRegistryEntry(Version: 8277, Type: typeof(PlayerRankingsPageMessage), Factory: PlayerRankingsPageMessage.Create)
        { CaptureName = "PlayerRankingsPageMessage" },

        [key: 23626] = new MessageRegistryEntry(Version: 0, Type: typeof(OutOfSyncMessage), Factory: OutOfSyncMessage.Create)
        { CaptureName = "OutOfSyncMessage" },
        [key: 23708] = new MessageRegistryEntry(Version: 0, Type: typeof(PlayerRankings23708Message), Factory: PlayerRankings23708Message.Create)
        { CaptureName = "PlayerRankings23708Message" },

        [key: 24149] = new MessageRegistryEntry(Version: 0, Type: typeof(AccountLoadResponseMessage), Factory: AccountLoadResponseMessage.Create)
        { CaptureName = "AccountLoadResponseMessage" },

        [key: 24180] = new MessageRegistryEntry(Version: 0, Type: typeof(OwnHomeDataMessage), Factory: OwnHomeDataMessage.Create)
        { CaptureName = "OwnHomeDataMessage" },

        [Clientbound28544MessageType] = new MessageRegistryEntry(Version: 0, Type: typeof(BaseHomeDataMessage), Factory: BaseHomeDataMessage.Create)
        { CaptureName = "Clientbound28544Message" },

        [key: 24489] = new MessageRegistryEntry(Version: 0, Type: typeof(OtherHomeDataMessage), Factory: OtherHomeDataMessage.Create)
        { CaptureName = "OtherHomeDataMessage" },

        [key: 24843] = new MessageRegistryEntry(Version: 0, Type: typeof(AccountCandidatesMessage), Factory: AccountCandidatesMessage.Create)
        { CaptureName = "Clientbound24843Message" },

        [key: 25220] = new MessageRegistryEntry(Version: 2, Type: typeof(LoginOkMessage), Factory: LoginOkMessage.Create)
        { CaptureName = "LoginOkMessage" },

        [key: 25892] = new MessageRegistryEntry(Version: 0, Type: typeof(DisconnectedMessage), Factory: DisconnectedMessage.Create)
        { CaptureName = "DisconnectedMessage" },

        [key: 26199] = new MessageRegistryEntry(Version: 0, Type: typeof(FriendMetadataMessage), Factory: FriendMetadataMessage.Create)
        { CaptureName = "Clientbound26199Message" },

        [key: 26385] = new MessageRegistryEntry(Version: 0, Type: typeof(Clientbound26385Message), Factory: Clientbound26385Message.Create)
        { CaptureName = "Clientbound26385Message" },

        [key: 27398] = new MessageRegistryEntry(Version: 0, Type: typeof(ResourceAssociationsMessage), Factory: ResourceAssociationsMessage.Create)
        { CaptureName = "Clientbound27398Message" },

        [key: 28061] = new MessageRegistryEntry(Version: 0, Type: typeof(Clientbound28061Message), Factory: Clientbound28061Message.Create)
        { CaptureName = "Clientbound28061Message" },

        [key: 28917] = new MessageRegistryEntry(Version: 0, Type: typeof(OtherFishingHomeDataMessage), Factory: OtherFishingHomeDataMessage.Create)
        { CaptureName = "OtherFishingHomeDataMessage" },

        [key: 29247] = new MessageRegistryEntry(Version: 0, Type: typeof(Clientbound29247Message), Factory: Clientbound29247Message.Create)
        { CaptureName = "Clientbound29247Message" },

        [key: 29415] = new MessageRegistryEntry(Version: 0, Type: typeof(Clientbound29415Message), Factory: Clientbound29415Message.Create)
        { CaptureName = "Clientbound29415Message" },

        [key: 29734] = new MessageRegistryEntry(Version: 0, Type: typeof(Clientbound29734Message), Factory: Clientbound29734Message.Create)
        { CaptureName = "Clientbound29734Message" },
    };
    /// <summary>Gets all registered packet contracts and their wire versions by identifier.</summary>
    public static IReadOnlyDictionary<ushort, MessageRegistryEntry> Registrations =>
        Map.AsReadOnly();

    /// <summary>Gets the stable capture label for a registered message or an opaque passthrough frame.</summary>
    public static string GetCaptureName(IMessage message)
    {
        ArgumentNullException.ThrowIfNull(message);

        return message is PassthroughMessage
            ? nameof(PassthroughMessage)
            : GetEntry(message.GetType()).CaptureName;
    }

    /// <summary>
    /// Gets <c language="csharp">Hint</c>.
    /// </summary>
    public static string? GetHint(ushort identifier)
    {
        return Hints.TryGetValue(identifier, out string? hint) ? hint : null;
    }

    /// <summary>
    /// Gets <c language="csharp">Id</c>.
    /// </summary>
    public static ushort GetIdentifier<TValue>(TValue message)
        where TValue : IMessage
    {
        return message is PassthroughMessage passthroughMessage ? passthroughMessage.Identifier : GetIdentifier(message.GetType());
    }

    /// <summary>
    /// Gets <c language="csharp">Id</c>.
    /// </summary>
    public static ushort GetIdentifier<TValue>()
        where TValue : IMessage
    {
        return GetIdentifier(typeof(TValue));
    }

    /// <summary>
    /// Gets <c language="csharp">Id</c>.
    /// </summary>
    public static ushort GetIdentifier(Type type)
    {
        MessageRegistryEntry entry = GetEntry(type);

        return Map.First(kv => kv.Value == entry).Key;
    }

    /// <summary>
    /// Gets <c language="csharp">Version</c>.
    /// </summary>
    public static ushort GetVersion<TValue>(TValue message)
        where TValue : IMessage
    {
        return message is PassthroughMessage passthroughMessage ? passthroughMessage.Version : GetVersion(message.GetType());
    }

    /// <summary>
    /// Gets <c language="csharp">Version</c>.
    /// </summary>
    public static ushort GetVersion<TValue>()
        where TValue : IMessage
    {
        return GetVersion(typeof(TValue));
    }

    /// <summary>
    /// Gets <c language="csharp">Version</c>.
    /// </summary>
    public static ushort GetVersion(Type type)
    {
        return GetEntry(type).Version;
    }

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
        ArgumentNullException.ThrowIfNull(container);

        return !Map.TryGetValue(container.Identifier, out MessageRegistryEntry? entry)
            ? PassthroughMessage.Create(container)
            : container.Identifier == GetIdentifier<EndClientTurnMessage>()
            ? EndClientTurnMessage.Create(container, CommandEnvironment.Production, dataResolver)
            : entry.Factory(container);
    }

    private static MessageRegistryEntry GetEntry(Type type)
    {
        return Map.Values.FirstOrDefault(entry => entry.Type == type)
            ?? throw new InvalidOperationException($"Message type {type} is not registered.");
    }
}
