using System.Collections.Frozen;

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
        [RequestGregFarmMessageType] = new MessageRegistryEntry(typeof(RequestGregFarmMessage), RequestGregFarmMessage.Decode)
        { CaptureName = nameof(RequestGregFarmMessage) },
        [key: 14049] = new MessageRegistryEntry(typeof(RequestOwnTownMessage), RequestOwnTownMessage.Decode)
        { CaptureName = nameof(RequestOwnTownMessage) },
        [key: 18475] = new MessageRegistryEntry(typeof(RequestOwnFishingHomeMessage), RequestOwnFishingHomeMessage.Decode)
        { CaptureName = nameof(RequestOwnFishingHomeMessage) },
        [key: 18335] = new MessageRegistryEntry(typeof(FollowMessage), FollowMessage.Decode)
        { CaptureName = nameof(FollowMessage) },
        [key: 14664] = new MessageRegistryEntry(typeof(UnfollowMessage), UnfollowMessage.Decode)
        { CaptureName = nameof(UnfollowMessage) },
        [key: 15117] = new MessageRegistryEntry(typeof(LikeFarmMessage), LikeFarmMessage.Decode)
        { CaptureName = nameof(LikeFarmMessage) },
        [key: 16037] = new MessageRegistryEntry(typeof(RequestFarmLikeStatusMessage), RequestFarmLikeStatusMessage.Decode)
        { CaptureName = nameof(RequestFarmLikeStatusMessage) },
        [key: 21236] = new MessageRegistryEntry(typeof(FollowResponseMessage), FollowResponseMessage.Decode)
        { CaptureName = nameof(FollowResponseMessage) },
        [key: 19845] = new MessageRegistryEntry(typeof(RequestFollowerListPageMessage), RequestFollowerListPageMessage.Decode)
        { CaptureName = nameof(RequestFollowerListPageMessage) },
        [key: 26605] = new MessageRegistryEntry(typeof(FollowerListPageMessage), FollowerListPageMessage.Decode)
        { CaptureName = nameof(FollowerListPageMessage) },
        [key: 18272] = new MessageRegistryEntry(typeof(RequestFollowerCountMessage), RequestFollowerCountMessage.Decode)
        { CaptureName = nameof(RequestFollowerCountMessage) },
        [key: 23455] = new MessageRegistryEntry(typeof(FollowerCountMessage), FollowerCountMessage.Decode)
        { CaptureName = nameof(FollowerCountMessage) },
        [key: 25679] = new MessageRegistryEntry(typeof(FriendCountMessage), FriendCountMessage.Decode)
        { CaptureName = nameof(FriendCountMessage) },
        [key: 26582] = new MessageRegistryEntry(typeof(FriendListUpdateMessage), FriendListUpdateMessage.Decode)
        { CaptureName = nameof(FriendListUpdateMessage) },
        [key: 22878] = new MessageRegistryEntry(typeof(RoadsidePurchaseResultMessage), RoadsidePurchaseResultMessage.Decode)
        { CaptureName = "RoadsidePurchaseResultMessage" },
        [key: 28562] = new MessageRegistryEntry(typeof(RoadsideListingBuyerMessage), RoadsideListingBuyerMessage.Decode)
        { CaptureName = "RoadsideListingBuyerMessage" },
        [key: 21767] = new MessageRegistryEntry(typeof(HomeLoadFailedMessage), HomeLoadFailedMessage.Decode)
        { CaptureName = "HomeLoadFailedMessage" },
        [key: 10100] = new MessageRegistryEntry(typeof(ClientHelloMessage), ClientHelloMessage.Decode)
        { CaptureName = "ClientHelloMessage" },

        [key: 10101] = new MessageRegistryEntry(typeof(LoginMessage), LoginMessage.Decode)
        { CaptureName = "LoginMessage" },

        [key: 10108] = new MessageRegistryEntry(typeof(KeepAliveMessage), KeepAliveMessage.Decode)
        { CaptureName = "KeepAliveMessage" },

        [key: 14484] = new MessageRegistryEntry(typeof(VisitHomeMessage), VisitHomeMessage.Decode)
        { CaptureName = "VisitHomeMessage" },

        [key: 17703] = new MessageRegistryEntry(typeof(VisitOtherFishingHomeMessage), VisitOtherFishingHomeMessage.Decode)
        { CaptureName = "VisitOtherFishingHomeMessage" },

        [key: 18671] = new MessageRegistryEntry(typeof(VisitHomeTargetMessage), VisitHomeTargetMessage.Decode)
        { CaptureName = "VisitHomeTargetMessage" },

        [key: 10224] = new MessageRegistryEntry(typeof(EndClientTurnMessage), EndClientTurnMessage.Decode)
        { CaptureName = "EndClientTurnMessage" },

        [key: 19949] = new MessageRegistryEntry(typeof(RequestOwnHomeMessage), RequestOwnHomeMessage.Decode)
        { CaptureName = "RequestOwnHomeMessage" },

        [key: 20013] = new MessageRegistryEntry(typeof(NeighborhoodListsMessage), NeighborhoodListsMessage.Decode)
        { CaptureName = "NeighborhoodListsMessage" },

        [key: 22158] = new MessageRegistryEntry(typeof(RoadsideBuyerMessage), RoadsideBuyerMessage.Decode)
        { CaptureName = "RoadsideBuyerMessage" },

        [key: 26668] = new MessageRegistryEntry(typeof(FarmLikeStatusMessage), FarmLikeStatusMessage.Decode)
        { CaptureName = "Clientbound26668Message" },

        [key: 20100] = new MessageRegistryEntry(typeof(ServerHelloMessage), ServerHelloMessage.Decode)
        { CaptureName = "ServerHelloMessage" },

        [key: 20103] = new MessageRegistryEntry(typeof(LoginFailedMessage), LoginFailedMessage.Decode)
        { CaptureName = "LoginFailedMessage" },

        [key: 20108] = new MessageRegistryEntry(typeof(KeepAliveOkMessage), KeepAliveOkMessage.Decode)
        { CaptureName = "KeepAliveOkMessage" },

        [key: 20155] = new MessageRegistryEntry(typeof(Clientbound20155Message), Clientbound20155Message.Decode)
        { CaptureName = "Clientbound20155Message" },

        [key: 20187] = new MessageRegistryEntry(typeof(AvailableServerCommandMessage), AvailableServerCommandMessage.Decode)
        { CaptureName = "AvailableServerCommandMessage" },

        [key: 20621] = new MessageRegistryEntry(typeof(Clientbound20621Message), Clientbound20621Message.Decode)
        { CaptureName = "Clientbound20621Message" },

        [key: 21915] = new MessageRegistryEntry(typeof(MailListMessage), MailListMessage.Decode)
        { CaptureName = nameof(MailListMessage) },

        [key: 21945] = new MessageRegistryEntry(typeof(Clientbound21945Message), Clientbound21945Message.Decode)
        { CaptureName = "Clientbound21945Message" },

        [key: 22903] = new MessageRegistryEntry(typeof(Clientbound22903Message), Clientbound22903Message.Decode)
        { CaptureName = "Clientbound22903Message" },
        [key: 28967] = new MessageRegistryEntry(typeof(NewspaperDataMessage), NewspaperDataMessage.Decode)
        { CaptureName = "NewspaperDataMessage" },
        [key: 26994] = new MessageRegistryEntry(typeof(Clientbound26994Message), Clientbound26994Message.Decode)
        { CaptureName = "Clientbound26994Message" },

        [key: 22302] = new MessageRegistryEntry(typeof(Clientbound22302Message), Clientbound22302Message.Decode)
        { CaptureName = "Clientbound22302Message" },

        [key: 22802] = new MessageRegistryEntry(typeof(Clientbound22802Message), Clientbound22802Message.Decode)
        { CaptureName = "Clientbound22802Message" },

        [key: 23074] = new MessageRegistryEntry(typeof(Clientbound23074Message), Clientbound23074Message.Decode)
        { CaptureName = "Clientbound23074Message" },

        [key: 23443] = new MessageRegistryEntry(typeof(PlayerRankingsMessage), PlayerRankingsMessage.Decode)
        { CaptureName = "PlayerRankingsMessage" },

        [key: 23444] = new MessageRegistryEntry(typeof(PlayerRankingsPageMessage), PlayerRankingsPageMessage.Decode)
        { CaptureName = "PlayerRankingsPageMessage" },

        [key: 23626] = new MessageRegistryEntry(typeof(OutOfSyncMessage), OutOfSyncMessage.Decode)
        { CaptureName = "OutOfSyncMessage" },
        [key: 23708] = new MessageRegistryEntry(typeof(PlayerRankings23708Message), PlayerRankings23708Message.Decode)
        { CaptureName = "PlayerRankings23708Message" },

        [key: 24149] = new MessageRegistryEntry(typeof(AccountLoadResponseMessage), AccountLoadResponseMessage.Decode)
        { CaptureName = "AccountLoadResponseMessage" },

        [key: 24180] = new MessageRegistryEntry(typeof(OwnHomeDataMessage), OwnHomeDataMessage.Decode)
        { CaptureName = "OwnHomeDataMessage" },

        [key: 24222] = new MessageRegistryEntry(typeof(FishingDataMessage), FishingDataMessage.Decode)
        { CaptureName = "FishingDataMessage" },

        [DecoCanvasDataMessageType] = new MessageRegistryEntry(typeof(DecoCanvasDataMessage), DecoCanvasDataMessage.Decode)
        { CaptureName = "DecoCanvasDataMessage" },

        [key: 20699] = new MessageRegistryEntry(typeof(GregFarmDataMessage), GregFarmDataMessage.Decode)
        { CaptureName = "GregFarmDataMessage" },

        [key: 24489] = new MessageRegistryEntry(typeof(OtherHomeDataMessage), OtherHomeDataMessage.Decode)
        { CaptureName = "OtherHomeDataMessage" },

        [key: 24843] = new MessageRegistryEntry(typeof(AccountCandidatesMessage), AccountCandidatesMessage.Decode)
        { CaptureName = "Clientbound24843Message" },

        [key: 25220] = new MessageRegistryEntry(typeof(LoginOkMessage), LoginOkMessage.Decode)
        { CaptureName = "LoginOkMessage" },

        [key: 25892] = new MessageRegistryEntry(typeof(DisconnectedMessage), DisconnectedMessage.Decode)
        { CaptureName = "DisconnectedMessage" },

        [key: 26199] = new MessageRegistryEntry(typeof(FriendMetadataMessage), FriendMetadataMessage.Decode)
        { CaptureName = "Clientbound26199Message" },

        [key: 26385] = new MessageRegistryEntry(typeof(PerformanceProfilingSettingsMessage), PerformanceProfilingSettingsMessage.Decode)
        { CaptureName = nameof(PerformanceProfilingSettingsMessage) },

        [key: 27398] = new MessageRegistryEntry(typeof(ResourceAssociationsMessage), ResourceAssociationsMessage.Decode)
        { CaptureName = "Clientbound27398Message" },

        [FeaturingDesignListMessageType] = new MessageRegistryEntry(typeof(FeaturingDesignListMessage), FeaturingDesignListMessage.Decode)
        { CaptureName = nameof(FeaturingDesignListMessage) },

        [DecorationGalleryDataMessageType] = new MessageRegistryEntry(typeof(DecorationGalleryDataMessage), DecorationGalleryDataMessage.Decode)
        { CaptureName = nameof(DecorationGalleryDataMessage) },

        [NeighborhoodFullListMessageType] = new MessageRegistryEntry(typeof(NeighborhoodFullListMessage), NeighborhoodFullListMessage.Decode)
        { CaptureName = nameof(NeighborhoodFullListMessage) },

        [AvatarStreamPageMessageType] = new MessageRegistryEntry(typeof(AvatarStreamPageMessage), AvatarStreamPageMessage.Decode)
        { CaptureName = nameof(AvatarStreamPageMessage) },

        [NeighborhoodChatMessageType] = new MessageRegistryEntry(typeof(NeighborhoodChatMessage), NeighborhoodChatMessage.Decode)
        { CaptureName = nameof(NeighborhoodChatMessage) },

        [NeighborhoodStreamEntryFlagMessageType] = new MessageRegistryEntry(typeof(NeighborhoodStreamEntryFlagMessage), NeighborhoodStreamEntryFlagMessage.Decode)
        { CaptureName = nameof(NeighborhoodStreamEntryFlagMessage) },

        [NeighborhoodMembersMessageType] = new MessageRegistryEntry(typeof(NeighborhoodMembersMessage), NeighborhoodMembersMessage.Decode)
        { CaptureName = nameof(NeighborhoodMembersMessage) },

        [OwnTownDataMessageType] = new MessageRegistryEntry(typeof(OwnTownDataMessage), OwnTownDataMessage.Decode)
        { CaptureName = nameof(OwnTownDataMessage) },

        [key: 28061] = new MessageRegistryEntry(typeof(Clientbound28061Message), Clientbound28061Message.Decode)
        { CaptureName = "Clientbound28061Message" },

        [key: 28917] = new MessageRegistryEntry(typeof(OtherFishingHomeDataMessage), OtherFishingHomeDataMessage.Decode)
        { CaptureName = "OtherFishingHomeDataMessage" },

        [key: 29247] = new MessageRegistryEntry(typeof(Clientbound29247Message), Clientbound29247Message.Decode)
        { CaptureName = "Clientbound29247Message" },

        [key: 29275] = new MessageRegistryEntry(typeof(ScidJwtMessage), ScidJwtMessage.Decode)
        { CaptureName = "ScidJwtMessage" },

        [key: 29415] = new MessageRegistryEntry(typeof(FriendListMessage), FriendListMessage.Decode)
        { CaptureName = nameof(FriendListMessage) },

        [key: 29734] = new MessageRegistryEntry(typeof(Clientbound29734Message), Clientbound29734Message.Decode)
        { CaptureName = "Clientbound29734Message" },
    };
    // The baseline above is retained once. Each revision starts at its first confirmed
    // release and remains in effect until the next revision for that message.
    // Set a revision's id to null to unregister the message from that release onward.
    private static readonly FrozenDictionary<Type, ProtocolIdChange[]> Changes = new Dictionary<Type, ProtocolIdChange[]>
    {
        [typeof(EndClientTurnMessage)] = [new(new Version(major: 1, minor: 73, build: 81), Id: 11657)],
        [typeof(PerformanceProfilingSettingsMessage)] = [new(PerformanceProfilingSettingsMessage.ReorderedFieldsVersion, Id: 22324)],
        [typeof(VisitHomeMessage)] = [new(new Version(major: 1, minor: 73, build: 81), Id: null)],
    }.ToFrozenDictionary();

    private static readonly FrozenDictionary<Type, MessageIdHistory> ByType = Map.ToFrozenDictionary(
        static pair => pair.Value.Type,
        static pair => new MessageIdHistory(pair.Value, new ProtocolIdHistory(pair.Key, Changes.GetValueOrDefault(pair.Value.Type) ?? []))
    );

    private static readonly FrozenDictionary<ushort, MessageIdHistory[]> ById = IndexIds();

    /// <summary>Gets the baseline packet contracts. Use GetId or Is with a game version for active ids.</summary>
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
    public static string? GetHint(ushort id)
    {
        return Hints.TryGetValue(id, out string? hint) ? hint : null;
    }

    /// <summary>
    /// Gets <c language="csharp">Id</c>.
    /// </summary>
    public static ushort GetId<TValue>(TValue message, Version? gameVersion = null)
        where TValue : IMessage
    {
        return message is PassthroughMessage passthroughMessage ? passthroughMessage.Id : GetId(message.GetType(), gameVersion);
    }

    /// <summary>
    /// Gets <c language="csharp">Id</c>.
    /// </summary>
    public static ushort GetId<TValue>(Version? gameVersion = null)
        where TValue : IMessage
    {
        return GetId(typeof(TValue), gameVersion);
    }

    /// <summary>
    /// Gets <c language="csharp">Id</c>.
    /// </summary>
    public static ushort GetId(Type type, Version? gameVersion = null)
    {
        return checked((ushort)(GetHistory(type).Ids.GetId(gameVersion)
            ?? throw new NotSupportedException($"Message type {type} is not registered for game version {gameVersion}.")));
    }

    /// <summary>Recognizes all registered wire ids belonging to the same message contract.</summary>
    public static bool Is<TMessage>(ushort id, Version? gameVersion = null) where TMessage : IMessage
    {
        return FindEntry(id, gameVersion)?.Type == typeof(TMessage);
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

        MessageRegistryEntry? entry = FindEntry(container.Id, container.Payload.GameVersion);

        return entry is null
            ? PassthroughMessage.Decode(container)
            : entry.Type == typeof(EndClientTurnMessage)
            ? EndClientTurnMessage.Decode(container.Payload, CommandEnvironment.Production, dataResolver)
            : entry.Type == typeof(AvailableServerCommandMessage)
            ? AvailableServerCommandMessage.Decode(container.Payload, dataResolver)
            : entry.Factory(container.Payload);
    }

    private static MessageRegistryEntry? FindEntry(ushort id, Version? gameVersion)
    {
        if (!ById.TryGetValue(id, out MessageIdHistory[]? histories))
            return null;

        MessageRegistryEntry? entry = null;

        foreach (MessageIdHistory history in histories)
        {
            if (gameVersion is not null && history.Ids.GetId(gameVersion) != id)
                continue;

            if (entry is not null)
                throw new InvalidDataException($"Packet id {id} has multiple message contracts. Game version: {gameVersion?.ToString() ?? "unknown"}.");

            entry = history.Entry;
        }

        return entry;
    }

    private static MessageRegistryEntry GetEntry(Type type)
    {
        return GetHistory(type).Entry;
    }

    private static MessageIdHistory GetHistory(Type type)
    {
        return ByType.TryGetValue(type, out MessageIdHistory? history)
            ? history
            : throw new InvalidOperationException($"Message type {type} is not registered.");
    }

    private static FrozenDictionary<ushort, MessageIdHistory[]> IndexIds()
    {
        Dictionary<ushort, List<MessageIdHistory>> index = [];

        foreach (KeyValuePair<Type, MessageIdHistory> registration in ByType)
        {
            MessageIdHistory history = registration.Value;

            foreach (int value in history.Ids.Ids.Distinct())
            {
                ushort id = checked((ushort)value);

                if (!index.TryGetValue(id, out List<MessageIdHistory>? histories))
                {
                    histories = [];
                    index.Add(id, histories);
                }

                histories.Add(history);
            }
        }

        return index.ToFrozenDictionary(static pair => pair.Key, static pair => pair.Value.ToArray());
    }
}
