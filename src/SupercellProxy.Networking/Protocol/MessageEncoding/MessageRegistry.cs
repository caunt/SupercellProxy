using SupercellProxy.Networking.Protocol.Accounts;
using SupercellProxy.Networking.Protocol.Authentication;
using SupercellProxy.Networking.Protocol.CollectionPayloads;
using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.ConnectionControl;
using SupercellProxy.Networking.Protocol.Events.Decoration;
using SupercellProxy.Networking.Protocol.Friends;
using SupercellProxy.Networking.Protocol.Friends.Entries;
using SupercellProxy.Networking.Protocol.Homes;
using SupercellProxy.Networking.Protocol.Homes.Requests;
using SupercellProxy.Networking.Protocol.Mail;
using SupercellProxy.Networking.Protocol.Neighborhoods;
using SupercellProxy.Networking.Protocol.Neighborhoods.Streams;
using SupercellProxy.Networking.Protocol.Neighborhoods.Chat;
using SupercellProxy.Networking.Protocol.Neighborhoods.Members;
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
    /// Identifies a clientbound avatar-stream page.
    public const ushort AvatarStreamPageMessageType = 26542;
    /// Identifies the clientbound deco-canvas home snapshot, loaded in native game mode 9.
    public const ushort DecoCanvasDataMessageType = 28544;

    /// Identifies clientbound decoration-gallery data for a home.
    public const ushort DecorationGalleryDataMessageType = 25133;

    /// Identifies the clientbound featured-decoration-design list.
    public const ushort FeaturingDesignListMessageType = 27413;

    /// Identifies the clientbound loading-complete gate used to initialize home turns.
    public const ushort HomeInitializationMessageType = 27439;

    /// <summary>Identifies a clientbound neighborhood chat entry.</summary>
    public const ushort NeighborhoodChatMessageType = 27910;

    /// Identifies clientbound full neighborhood profiles.
    public const ushort NeighborhoodFullListMessageType = 29897;

    /// <summary>Identifies a clientbound neighborhood member list.</summary>
    public const ushort NeighborhoodMembersMessageType = 28583;

    /// <summary>Marks the second state flag on one Neighborhood stream entry.</summary>
    public const ushort NeighborhoodStreamEntryFlagMessageType = 23867;

    /// <summary>Identifies the player's town home snapshot.</summary>
    public const ushort OwnTownDataMessageType = 28543;

    /// <summary>Requests Greg's farm in the selected language.</summary>
    public const ushort RequestGregFarmMessageType = 14038;

    /// Identifies the serverbound startup pulse observed before a zero-command home turn.
    public const ushort Serverbound38101MessageType = 38101;

    private static readonly Dictionary<ushort, string> Hints = new()
    {
        [key: 10518] = "open friend book",
        [key: 14972] = "last helpers request",
        [key: 20155] = "???",
        [key: 21628] = "last helpers response",
        [key: 24180] = "OWN_HOME_DATA",
        [key: 26199] = "LogicArrayList<FriendMeta *>",
        [key: 40000] = "updateConversionValue",
    };

    private static readonly Dictionary<ushort, MessageRegistryEntry> Map = new()
    {
        [RequestGregFarmMessageType] = new MessageRegistryEntry(typeof(RequestGregFarmMessage), RequestGregFarmMessage.Create)
        { CaptureName = nameof(RequestGregFarmMessage) },
        [key: 14049] = new MessageRegistryEntry(typeof(RequestOwnTownMessage), RequestOwnTownMessage.Create)
        { CaptureName = nameof(RequestOwnTownMessage) },
        [key: 18475] = new MessageRegistryEntry(typeof(RequestOwnFishingHomeMessage), RequestOwnFishingHomeMessage.Create)
        { CaptureName = nameof(RequestOwnFishingHomeMessage) },
        [key: 18335] = new MessageRegistryEntry(typeof(FollowMessage), FollowMessage.Create)
        { CaptureName = nameof(FollowMessage) },
        [key: 21236] = new MessageRegistryEntry(typeof(FollowResponseMessage), FollowResponseMessage.Create)
        { CaptureName = nameof(FollowResponseMessage) },
        [key: 19845] = new MessageRegistryEntry(typeof(RequestFollowerListPageMessage), RequestFollowerListPageMessage.Create)
        { CaptureName = nameof(RequestFollowerListPageMessage) },
        [key: 26605] = new MessageRegistryEntry(typeof(FollowerListPageMessage), FollowerListPageMessage.Create)
        { CaptureName = nameof(FollowerListPageMessage) },
        [key: 18272] = new MessageRegistryEntry(typeof(RequestFollowerCountMessage), RequestFollowerCountMessage.Create)
        { CaptureName = nameof(RequestFollowerCountMessage) },
        [key: 23455] = new MessageRegistryEntry(typeof(FollowerCountMessage), FollowerCountMessage.Create)
        { CaptureName = nameof(FollowerCountMessage) },
        [key: 25679] = new MessageRegistryEntry(typeof(FriendCountMessage), FriendCountMessage.Create)
        { CaptureName = nameof(FriendCountMessage) },
        [key: 26582] = new MessageRegistryEntry(typeof(FriendListUpdateMessage), FriendListUpdateMessage.Create)
        { CaptureName = nameof(FriendListUpdateMessage) },
        [key: 22878] = new MessageRegistryEntry(typeof(RoadsidePurchaseResultMessage), RoadsidePurchaseResultMessage.Create)
        { CaptureName = "RoadsidePurchaseResultMessage" },
        [key: 28562] = new MessageRegistryEntry(typeof(RoadsideListingBuyerMessage), RoadsideListingBuyerMessage.Create)
        { CaptureName = "RoadsideListingBuyerMessage" },
        [key: 21767] = new MessageRegistryEntry(typeof(HomeLoadFailedMessage), HomeLoadFailedMessage.Create)
        { CaptureName = "HomeLoadFailedMessage" },
        [key: 10100] = new MessageRegistryEntry(typeof(ClientHelloMessage), ClientHelloMessage.Create)
        { CaptureName = "ClientHelloMessage" },

        [key: 10101] = new MessageRegistryEntry(typeof(LoginMessage), LoginMessage.Create)
        { CaptureName = "LoginMessage" },

        [key: 10108] = new MessageRegistryEntry(typeof(KeepAliveMessage), KeepAliveMessage.Create)
        { CaptureName = "KeepAliveMessage" },

        [key: 14484] = new MessageRegistryEntry(typeof(VisitHomeMessage), VisitHomeMessage.Create)
        { CaptureName = "VisitHomeMessage" },

        [key: 17703] = new MessageRegistryEntry(typeof(VisitOtherFishingHomeMessage), VisitOtherFishingHomeMessage.Create)
        { CaptureName = "VisitOtherFishingHomeMessage" },

        [key: 18671] = new MessageRegistryEntry(typeof(VisitHomeTargetMessage), VisitHomeTargetMessage.Create)
        { CaptureName = "VisitHomeTargetMessage" },

        [key: 10224] = new MessageRegistryEntry(typeof(EndClientTurnMessage), EndClientTurnMessage.Create)
        { CaptureName = "EndClientTurnMessage" },

        [key: 19949] = new MessageRegistryEntry(typeof(RequestOwnHomeMessage), RequestOwnHomeMessage.Create)
        { CaptureName = "RequestOwnHomeMessage" },

        [key: 20013] = new MessageRegistryEntry(typeof(NeighborhoodListsMessage), NeighborhoodListsMessage.Create)
        { CaptureName = "NeighborhoodListsMessage" },

        [key: 22158] = new MessageRegistryEntry(typeof(RoadsideBuyerMessage), RoadsideBuyerMessage.Create)
        { CaptureName = "RoadsideBuyerMessage" },

        [key: 26668] = new MessageRegistryEntry(typeof(HomeVisitStatusMessage), HomeVisitStatusMessage.Create)
        { CaptureName = "Clientbound26668Message" },

        [key: 20100] = new MessageRegistryEntry(typeof(ServerHelloMessage), ServerHelloMessage.Create)
        { CaptureName = "ServerHelloMessage" },

        [key: 20103] = new MessageRegistryEntry(typeof(LoginFailedMessage), LoginFailedMessage.Create)
        { CaptureName = "LoginFailedMessage" },

        [key: 20108] = new MessageRegistryEntry(typeof(KeepAliveOkMessage), KeepAliveOkMessage.Create)
        { CaptureName = "KeepAliveOkMessage" },

        [key: 20155] = new MessageRegistryEntry(typeof(Clientbound20155Message), Clientbound20155Message.Create)
        { CaptureName = "Clientbound20155Message" },

        [key: 20187] = new MessageRegistryEntry(typeof(AvailableServerCommandMessage), AvailableServerCommandMessage.Create)
        { CaptureName = "AvailableServerCommandMessage" },

        [key: 20621] = new MessageRegistryEntry(typeof(Clientbound20621Message), Clientbound20621Message.Create)
        { CaptureName = "Clientbound20621Message" },

        [key: 21915] = new MessageRegistryEntry(typeof(MailListMessage), MailListMessage.Create)
        { CaptureName = nameof(MailListMessage) },

        [key: 21945] = new MessageRegistryEntry(typeof(Clientbound21945Message), Clientbound21945Message.Create)
        { CaptureName = "Clientbound21945Message" },

        [key: 22903] = new MessageRegistryEntry(typeof(Clientbound22903Message), Clientbound22903Message.Create)
        { CaptureName = "Clientbound22903Message" },
        [key: 28967] = new MessageRegistryEntry(typeof(NewspaperDataMessage), NewspaperDataMessage.Create)
        { CaptureName = "NewspaperDataMessage" },
        [key: 26994] = new MessageRegistryEntry(typeof(Clientbound26994Message), Clientbound26994Message.Create)
        { CaptureName = "Clientbound26994Message" },

        [key: 22302] = new MessageRegistryEntry(typeof(Clientbound22302Message), Clientbound22302Message.Create)
        { CaptureName = "Clientbound22302Message" },

        [key: 22802] = new MessageRegistryEntry(typeof(Clientbound22802Message), Clientbound22802Message.Create)
        { CaptureName = "Clientbound22802Message" },

        [key: 23074] = new MessageRegistryEntry(typeof(Clientbound23074Message), Clientbound23074Message.Create)
        { CaptureName = "Clientbound23074Message" },

        [key: 23443] = new MessageRegistryEntry(typeof(PlayerRankingsMessage), PlayerRankingsMessage.Create)
        { CaptureName = "PlayerRankingsMessage" },

        [key: 23444] = new MessageRegistryEntry(typeof(PlayerRankingsPageMessage), PlayerRankingsPageMessage.Create)
        { CaptureName = "PlayerRankingsPageMessage" },

        [key: 23626] = new MessageRegistryEntry(typeof(OutOfSyncMessage), OutOfSyncMessage.Create)
        { CaptureName = "OutOfSyncMessage" },
        [key: 23708] = new MessageRegistryEntry(typeof(PlayerRankings23708Message), PlayerRankings23708Message.Create)
        { CaptureName = "PlayerRankings23708Message" },

        [key: 24149] = new MessageRegistryEntry(typeof(AccountLoadResponseMessage), AccountLoadResponseMessage.Create)
        { CaptureName = "AccountLoadResponseMessage" },

        [key: 24180] = new MessageRegistryEntry(typeof(OwnHomeDataMessage), OwnHomeDataMessage.Create)
        { CaptureName = "OwnHomeDataMessage" },

        [key: 24222] = new MessageRegistryEntry(typeof(FishingDataMessage), FishingDataMessage.Create)
        { CaptureName = "FishingDataMessage" },

        [DecoCanvasDataMessageType] = new MessageRegistryEntry(typeof(DecoCanvasDataMessage), DecoCanvasDataMessage.Create)
        { CaptureName = "DecoCanvasDataMessage" },

        [key: 20699] = new MessageRegistryEntry(typeof(GregFarmDataMessage), GregFarmDataMessage.Create)
        { CaptureName = "GregFarmDataMessage" },

        [key: 24489] = new MessageRegistryEntry(typeof(OtherHomeDataMessage), OtherHomeDataMessage.Create)
        { CaptureName = "OtherHomeDataMessage" },

        [key: 24843] = new MessageRegistryEntry(typeof(AccountCandidatesMessage), AccountCandidatesMessage.Create)
        { CaptureName = "Clientbound24843Message" },

        [key: 25220] = new MessageRegistryEntry(typeof(LoginOkMessage), LoginOkMessage.Create)
        { CaptureName = "LoginOkMessage" },

        [key: 25892] = new MessageRegistryEntry(typeof(DisconnectedMessage), DisconnectedMessage.Create)
        { CaptureName = "DisconnectedMessage" },

        [key: 26199] = new MessageRegistryEntry(typeof(FriendMetadataMessage), FriendMetadataMessage.Create)
        { CaptureName = "Clientbound26199Message" },

        [key: 26385] = new MessageRegistryEntry(typeof(Clientbound26385Message), Clientbound26385Message.Create)
        { CaptureName = "Clientbound26385Message" },

        [key: 27398] = new MessageRegistryEntry(typeof(ResourceAssociationsMessage), ResourceAssociationsMessage.Create)
        { CaptureName = "Clientbound27398Message" },

        [FeaturingDesignListMessageType] = new MessageRegistryEntry(typeof(FeaturingDesignListMessage), FeaturingDesignListMessage.Create)
        { CaptureName = nameof(FeaturingDesignListMessage) },

        [DecorationGalleryDataMessageType] = new MessageRegistryEntry(typeof(DecorationGalleryDataMessage), DecorationGalleryDataMessage.Create)
        { CaptureName = nameof(DecorationGalleryDataMessage) },

        [NeighborhoodFullListMessageType] = new MessageRegistryEntry(typeof(NeighborhoodFullListMessage), NeighborhoodFullListMessage.Create)
        { CaptureName = nameof(NeighborhoodFullListMessage) },

        [AvatarStreamPageMessageType] = new MessageRegistryEntry(typeof(AvatarStreamPageMessage), AvatarStreamPageMessage.Create)
        { CaptureName = nameof(AvatarStreamPageMessage) },

        [NeighborhoodChatMessageType] = new MessageRegistryEntry(typeof(NeighborhoodChatMessage), NeighborhoodChatMessage.Create)
        { CaptureName = nameof(NeighborhoodChatMessage) },

        [NeighborhoodStreamEntryFlagMessageType] = new MessageRegistryEntry(typeof(NeighborhoodStreamEntryFlagMessage), NeighborhoodStreamEntryFlagMessage.Create)
        { CaptureName = nameof(NeighborhoodStreamEntryFlagMessage) },

        [NeighborhoodMembersMessageType] = new MessageRegistryEntry(typeof(NeighborhoodMembersMessage), NeighborhoodMembersMessage.Create)
        { CaptureName = nameof(NeighborhoodMembersMessage) },

        [OwnTownDataMessageType] = new MessageRegistryEntry(typeof(OwnTownDataMessage), OwnTownDataMessage.Create)
        { CaptureName = nameof(OwnTownDataMessage) },

        [key: 28061] = new MessageRegistryEntry(typeof(Clientbound28061Message), Clientbound28061Message.Create)
        { CaptureName = "Clientbound28061Message" },

        [key: 28917] = new MessageRegistryEntry(typeof(OtherFishingHomeDataMessage), OtherFishingHomeDataMessage.Create)
        { CaptureName = "OtherFishingHomeDataMessage" },

        [key: 29247] = new MessageRegistryEntry(typeof(Clientbound29247Message), Clientbound29247Message.Create)
        { CaptureName = "Clientbound29247Message" },

        [key: 29275] = new MessageRegistryEntry(typeof(ScidJwtMessage), ScidJwtMessage.Create)
        { CaptureName = "ScidJwtMessage" },

        [key: 29415] = new MessageRegistryEntry(typeof(FriendListMessage), FriendListMessage.Create)
        { CaptureName = nameof(FriendListMessage) },

        [key: 29734] = new MessageRegistryEntry(typeof(Clientbound29734Message), Clientbound29734Message.Create)
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
            : container.Identifier == GetIdentifier<AvailableServerCommandMessage>()
            ? AvailableServerCommandMessage.Create(container, dataResolver)
            : entry.Factory(container);
    }

    private static MessageRegistryEntry GetEntry(Type type)
    {
        return Map.Values.FirstOrDefault(entry => entry.Type == type)
            ?? throw new InvalidOperationException($"Message type {type} is not registered.");
    }
}
