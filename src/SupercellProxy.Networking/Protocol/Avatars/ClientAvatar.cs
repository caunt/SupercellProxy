using SupercellProxy.Networking.Protocol.Avatars.Collections;
using SupercellProxy.Networking.Protocol.Inventory;
using SupercellProxy.Networking.Protocol.Mail;
using SupercellProxy.Networking.Protocol.MapGame;
using SupercellProxy.Networking.Protocol.Neighborhoods;
using SupercellProxy.Networking.Protocol.RoadsideShops;
using SupercellProxy.Networking.Protocol.Town;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>
/// Represents <c language="csharp">ClientAvatar</c>.
/// </summary>
public sealed record ClientAvatar
{
    /// <summary>Number of encoded inventory arrays.</summary>
    public const int InventoryArrayCount = 93;

    /// <summary>Number of encoded inventory maps.</summary>
    public const int InventoryMapCount = 3;

    /// <summary>Length of the first fixed progression group.</summary>
    public const int UnknownValues1Count = 11;

    /// <summary>Length of the second fixed progression group.</summary>
    public const int UnknownValues2Count = 6;

    /// <summary>
    /// Gets or sets the <c language="csharp">AccountId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("AccountId")]
    public LongIdentifier AccountIdentifier { get; init; }

    /// Gets the country code used by age restrictions.
    public string? AgeCountryCode { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">AvatarVersion</c> value.
    /// </summary>
    public int AvatarVersion { get; init; }

    /// Gets the retained birth timestamp used by age restrictions.
    public int BirthTimestamp { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">CanEditFarm</c> value.
    /// </summary>
    public bool CanEditFarm { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">DeprecatedInventoryDataCount</c> value.
    /// </summary>
    public int DeprecatedInventoryDataCount { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">HomeId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("HomeId")]
    public LongIdentifier HomeIdentifier { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">InventoryValues</c> value.
    /// </summary>
    public int[][] InventoryValues { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">InventoryUnknown0</c> value.
    /// </summary>
    public int InventoryUnknown0 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">InventoryMaps</c> value.
    /// </summary>
    public DataReferenceValue[][] InventoryMaps { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">IsMuted</c> value.
    /// </summary>
    public bool IsMuted { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">LeagueScore</c> value.
    /// </summary>
    public int LeagueScore { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">LeagueType</c> value.
    /// </summary>
    public int LeagueType { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">RoadsideShop</c> value.
    /// </summary>
    public RoadsideShopEntry[] RoadsideShop { get; set; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">MapGameId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("MapGameId")]
    public LongIdentifier? MapGameIdentifier { get; init; }

    /// <summary>
    /// Gets the Map Game Participants value.
    /// </summary>
    public MapGameParticipant[]? MapGameParticipants { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Name</c> value.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Neighborhood</c> value.
    /// </summary>
    public NeighborhoodData? Neighborhood { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">MailEntries</c> value.
    /// </summary>
    public MailEntry[] MailEntries { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownValues0</c> value.
    /// </summary>
    public int[] UnknownValues0 { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">Settings</c> value.
    /// </summary>
    public AvatarSettings? Settings { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">StorePromotionAllowed</c> value.
    /// </summary>
    public bool StorePromotionAllowed { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">TrainStationReady</c> value.
    /// </summary>
    public bool TrainStationReady { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown0</c> value.
    /// </summary>
    public int Unknown0 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown1</c> value.
    /// </summary>
    public int Unknown1 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown3</c> value.
    /// </summary>
    public int Unknown3 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown4</c> value.
    /// </summary>
    public int Unknown4 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownBoolean1</c> value.
    /// </summary>
    public bool UnknownBoolean1 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownEntries0</c> value.
    /// </summary>
    public AvatarIdentifierTripleEntry[] UnknownEntries0 { get; set; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownEntries1</c> value.
    /// </summary>
    public AvatarIdentifierFlagEntry[] UnknownEntries1 { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">PickedPassengers</c> value.
    /// </summary>
    public PickedPassenger[] PickedPassengers { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownEntries2</c> value.
    /// </summary>
    public AvatarIdentifierPairEntry[] UnknownEntries2 { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownLeagueValue</c> value.
    /// </summary>
    public int UnknownLeagueValue { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownEntries3</c> value.
    /// </summary>
    public AvatarIdentifierPairEntry[] UnknownEntries3 { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownValues1</c> value.
    /// </summary>
    public int[] UnknownValues1 { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownManager0</c> value.
    /// </summary>
    public AvatarCollectionSection UnknownManager0 { get; init; } = new();

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownNullableListCount</c> value.
    /// </summary>
    public int UnknownNullableListCount { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownOptionalId0</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownOptionalId0")]
    public LongIdentifier? UnknownOptionalIdentifier0 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownOptionalId1</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownOptionalId1")]
    public LongIdentifier? UnknownOptionalIdentifier1 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownOptionalId3</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownOptionalId3")]
    public LongIdentifier? UnknownOptionalIdentifier3 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownString1</c> value.
    /// </summary>
    public string? UnknownString1 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownManager1</c> value.
    /// </summary>
    public AvatarStringSection UnknownManager1 { get; init; } = new();

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownValues2</c> value.
    /// </summary>
    public int[] UnknownValues2 { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownManager2</c> value.
    /// </summary>
    public AvatarStateSection UnknownManager2 { get; init; } = new();

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static ClientAvatar Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int unknown0 = stream.ReadVariableInt();
        int unknown1 = stream.ReadVariableInt();
        int avatarVersion = stream.ReadVariableInt();
        int unknown3 = stream.ReadVariableInt();
        string? name = stream.ReadOptionalString();
        LongIdentifier homeIdentifier = stream.ReadLongIdentifier();
        LongIdentifier accountIdentifier = stream.ReadLongIdentifier();
        (int[][] Values, DataReferenceValue[][] Maps, int DeprecatedDataCount, int Unknown0) inventory = DecodeInventory(stream);
        (RoadsideShopEntry[] RoadsideShop, NeighborhoodData? Neighborhood, MailEntry[] MailEntries, int[] UnknownValues0, AvatarIdentifierTripleEntry[] UnknownEntries0, bool TrainStationReady, bool IsMuted, bool CanEditFarm, AvatarIdentifierFlagEntry[] UnknownEntries1, PickedPassenger[] PickedPassengers, AvatarIdentifierPairEntry[] UnknownEntries2, AvatarIdentifierPairEntry[] UnknownEntries3) social = DecodeSocialState(stream);
        (int UnknownNullableListCount, LongIdentifier? UnknownOptionalId0, LongIdentifier? UnknownOptionalId1, int LeagueType, int UnknownLeagueValue, int LeagueScore, int[] UnknownValues1, AvatarCollectionSection UnknownManager0, AvatarStringSection UnknownManager1, int[] UnknownValues2, LongIdentifier? MapGameId, LongIdentifier? UnknownOptionalId3, int Unknown4, MapGameParticipant[]? MapGameParticipants, int BirthTimestamp, string? AgeCountryCode, bool StorePromotionAllowed, string? UnknownString1, bool UnknownBoolean1, AvatarStateSection UnknownManager2, AvatarSettings? Settings) progression = DecodeProgressionState(stream);

        return new ClientAvatar
        {
            Unknown0 = unknown0,
            Unknown1 = unknown1,
            AvatarVersion = avatarVersion,
            Unknown3 = unknown3,
            Name = name,
            HomeIdentifier = homeIdentifier,
            AccountIdentifier = accountIdentifier,
            InventoryValues = inventory.Values,
            InventoryMaps = inventory.Maps,
            DeprecatedInventoryDataCount = inventory.DeprecatedDataCount,
            InventoryUnknown0 = inventory.Unknown0,
            RoadsideShop = social.RoadsideShop,
            Neighborhood = social.Neighborhood,
            MailEntries = social.MailEntries,
            UnknownValues0 = social.UnknownValues0,
            UnknownEntries0 = social.UnknownEntries0,
            TrainStationReady = social.TrainStationReady,
            IsMuted = social.IsMuted,
            CanEditFarm = social.CanEditFarm,
            UnknownEntries1 = social.UnknownEntries1,
            PickedPassengers = social.PickedPassengers,
            UnknownEntries2 = social.UnknownEntries2,
            UnknownEntries3 = social.UnknownEntries3,
            UnknownNullableListCount = progression.UnknownNullableListCount,
            UnknownOptionalIdentifier0 = progression.UnknownOptionalId0,
            UnknownOptionalIdentifier1 = progression.UnknownOptionalId1,
            LeagueType = progression.LeagueType,
            UnknownLeagueValue = progression.UnknownLeagueValue,
            LeagueScore = progression.LeagueScore,
            UnknownValues1 = progression.UnknownValues1,
            UnknownManager0 = progression.UnknownManager0,
            UnknownManager1 = progression.UnknownManager1,
            UnknownValues2 = progression.UnknownValues2,
            MapGameIdentifier = progression.MapGameId,
            MapGameParticipants = progression.MapGameParticipants,
            UnknownOptionalIdentifier3 = progression.UnknownOptionalId3,
            Unknown4 = progression.Unknown4,
            BirthTimestamp = progression.BirthTimestamp,
            AgeCountryCode = progression.AgeCountryCode,
            StorePromotionAllowed = progression.StorePromotionAllowed,
            UnknownString1 = progression.UnknownString1,
            UnknownBoolean1 = progression.UnknownBoolean1,
            UnknownManager2 = progression.UnknownManager2,
            Settings = progression.Settings,
        };
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        ValidateEncodableState();
        EncodeIdentityAndInventory(stream);
        EncodeSocialState(stream);
        EncodeProgressionState(stream);
    }

    private static (
        int[][] Values,
        DataReferenceValue[][] Maps,
        int DeprecatedDataCount,
        int Unknown0
    ) DecodeInventory(MessageStream stream)
    {
        int[][] values = new int[InventoryArrayCount][];

        for (int index = 0; index < values.Length; index++)
            values[index] = stream.ReadArray(static valueStream => valueStream.ReadVariableInt());

        DataReferenceValue[][] maps = new DataReferenceValue[InventoryMapCount][];

        for (int index = 0; index < maps.Length; index++)
            maps[index] = stream.ReadArray(DataReferenceValue.Decode);

        int deprecatedDataCount = stream.ReadVariableInt();

        return deprecatedDataCount is not 0
            ? throw new InvalidDataException(message: "The deprecated polymorphic inventory section is not implemented.")
            : ((int[][] Values, DataReferenceValue[][] Maps, int DeprecatedDataCount, int Unknown0))(values, maps, deprecatedDataCount, stream.ReadVariableInt());
    }

    private static (
        int UnknownNullableListCount,
        LongIdentifier? UnknownOptionalId0,
        LongIdentifier? UnknownOptionalId1,
        int LeagueType,
        int UnknownLeagueValue,
        int LeagueScore,
        int[] UnknownValues1,
        AvatarCollectionSection UnknownManager0,
        AvatarStringSection UnknownManager1,
        int[] UnknownValues2,
        LongIdentifier? MapGameId,
        LongIdentifier? UnknownOptionalId3,
        int Unknown4,
        MapGameParticipant[]? MapGameParticipants,
        int BirthTimestamp,
        string? AgeCountryCode,
        bool StorePromotionAllowed,
        string? UnknownString1,
        bool UnknownBoolean1,
        AvatarStateSection UnknownManager2,
        AvatarSettings? Settings
    ) DecodeProgressionState(MessageStream stream)
    {
        int unknownNullableListCount = stream.ReadVariableInt();

        if (unknownNullableListCount > 0)
            throw new InvalidDataException(message: "The nullable polymorphic avatar section is not implemented.");

        LongIdentifier? unknownOptionalIdentifier0 = stream.ReadOptionalLongIdentifier();
        LongIdentifier? unknownOptionalIdentifier1 = stream.ReadOptionalLongIdentifier();
        int leagueType = stream.ReadVariableInt();
        int unknownLeagueValue = stream.ReadVariableInt();
        int leagueScore = stream.ReadVariableInt();
        int[] unknownValues1 = stream.ReadVariableIntArray(count: 11);
        AvatarCollectionSection unknownManager0 = AvatarCollectionSection.Decode(stream);
        AvatarStringSection unknownManager1 = AvatarStringSection.Decode(stream);
        int[] unknownValues2 = stream.ReadVariableIntArray(count: 6);
        LongIdentifier? mapGameIdentifier = stream.ReadOptionalLongIdentifier();
        LongIdentifier? unknownOptionalIdentifier3 = stream.ReadOptionalLongIdentifier();
        int unknown4 = stream.ReadVariableInt();

        MapGameParticipant[]? participants =
            mapGameIdentifier is not null && stream.ReadBoolean()
                ? stream.ReadArray(MapGameParticipant.Decode)
                : null;

        return (
            unknownNullableListCount,
            unknownOptionalIdentifier0,
            unknownOptionalIdentifier1,
            leagueType,
            unknownLeagueValue,
            leagueScore,
            unknownValues1,
            unknownManager0,
            unknownManager1,
            unknownValues2,
            mapGameIdentifier,
            unknownOptionalIdentifier3,
            unknown4,
            participants,
            stream.ReadVariableInt(),
            stream.ReadOptionalString(),
            stream.ReadBoolean(),
            stream.ReadOptionalString(),
            stream.ReadBoolean(),
            AvatarStateSection.Decode(stream),
            stream.ReadBoolean() ? AvatarSettings.Decode(stream) : null
        );
    }

    private static (
        RoadsideShopEntry[] RoadsideShop,
        NeighborhoodData? Neighborhood,
        MailEntry[] MailEntries,
        int[] UnknownValues0,
        AvatarIdentifierTripleEntry[] UnknownEntries0,
        bool TrainStationReady,
        bool IsMuted,
        bool CanEditFarm,
        AvatarIdentifierFlagEntry[] UnknownEntries1,
        PickedPassenger[] PickedPassengers,
        AvatarIdentifierPairEntry[] UnknownEntries2,
        AvatarIdentifierPairEntry[] UnknownEntries3
    ) DecodeSocialState(MessageStream stream)
    {
        return (
            stream.ReadArray(RoadsideShopEntry.Decode),
            stream.ReadBoolean() ? NeighborhoodData.Decode(stream) : null,
            stream.ReadArray(MailEntry.Decode),
            stream.ReadArray(static valueStream => valueStream.ReadVariableInt()),
            stream.ReadArray(AvatarIdentifierTripleEntry.Decode),
            stream.ReadBoolean(),
            stream.ReadBoolean(),
            stream.ReadBoolean(),
            stream.ReadArray(AvatarIdentifierFlagEntry.Decode),
            stream.ReadArray(PickedPassenger.Decode),
            stream.ReadArray(AvatarIdentifierPairEntry.Decode),
            stream.ReadArray(AvatarIdentifierPairEntry.Decode)
        );
    }

    private void EncodeIdentityAndInventory(MessageStream stream)
    {
        stream.WriteVariableInt(Unknown0);
        stream.WriteVariableInt(Unknown1);
        stream.WriteVariableInt(AvatarVersion);
        stream.WriteVariableInt(Unknown3);
        stream.WriteOptionalString(Name);
        stream.WriteLongIdentifier(HomeIdentifier);
        stream.WriteLongIdentifier(AccountIdentifier);

        foreach (int[] values in InventoryValues)
            stream.WriteArray(values, static (valueStream, value) => valueStream.WriteVariableInt(value));

        foreach (DataReferenceValue[] values in InventoryMaps)
            stream.WriteArray(values, static (valueStream, value) => value.Encode(valueStream));

        stream.WriteVariableInt(DeprecatedInventoryDataCount);
        stream.WriteVariableInt(InventoryUnknown0);
    }

    private void EncodeProgressionState(MessageStream stream)
    {
        stream.WriteVariableInt(UnknownNullableListCount);
        stream.WriteOptionalLongIdentifier(UnknownOptionalIdentifier0);
        stream.WriteOptionalLongIdentifier(UnknownOptionalIdentifier1);
        stream.WriteVariableInt(LeagueType);
        stream.WriteVariableInt(UnknownLeagueValue);
        stream.WriteVariableInt(LeagueScore);

        foreach (int value in UnknownValues1)
            stream.WriteVariableInt(value);

        UnknownManager0.Encode(stream);
        UnknownManager1.Encode(stream);

        foreach (int value in UnknownValues2)
            stream.WriteVariableInt(value);

        stream.WriteOptionalLongIdentifier(MapGameIdentifier);
        stream.WriteOptionalLongIdentifier(UnknownOptionalIdentifier3);
        stream.WriteVariableInt(Unknown4);

        if (MapGameIdentifier is not null)
        {
            stream.WriteBoolean(MapGameParticipants is not null);

            if (MapGameParticipants is { } participants)
                stream.WriteArray(participants, static (output, value) => value.Encode(output));
        }

        stream.WriteVariableInt(BirthTimestamp);
        stream.WriteOptionalString(AgeCountryCode);
        stream.WriteBoolean(StorePromotionAllowed);
        stream.WriteOptionalString(UnknownString1);
        stream.WriteBoolean(UnknownBoolean1);
        UnknownManager2.Encode(stream);
        stream.WriteBoolean(Settings is not null);
        Settings?.Encode(stream);
    }

    private void EncodeSocialState(MessageStream stream)
    {
        stream.WriteArray(RoadsideShop, static (valueStream, value) => value.Encode(valueStream));
        stream.WriteBoolean(Neighborhood is not null);
        Neighborhood?.Encode(stream);
        stream.WriteArray(MailEntries, static (valueStream, value) => value.Encode(valueStream));
        stream.WriteArray(UnknownValues0, static (valueStream, value) => valueStream.WriteVariableInt(value));
        stream.WriteArray(UnknownEntries0, static (valueStream, value) => value.Encode(valueStream));
        stream.WriteBoolean(TrainStationReady);
        stream.WriteBoolean(IsMuted);
        stream.WriteBoolean(CanEditFarm);
        stream.WriteArray(UnknownEntries1, static (valueStream, value) => value.Encode(valueStream));
        stream.WriteArray(PickedPassengers, static (valueStream, value) => value.Encode(valueStream));
        stream.WriteArray(UnknownEntries2, static (valueStream, value) => value.Encode(valueStream));
        stream.WriteArray(UnknownEntries3, static (valueStream, value) => value.Encode(valueStream));
    }

    private void ValidateEncodableState()
    {
        if (InventoryValues.Length != InventoryArrayCount || InventoryMaps.Length != InventoryMapCount)
            throw new InvalidOperationException(message: "Unexpected inventory field count.");

        if (DeprecatedInventoryDataCount is not 0 || UnknownNullableListCount > 0)
            throw new InvalidOperationException(message: "Cannot encode an unsupported avatar section.");

        if (UnknownValues1.Length != UnknownValues1Count || UnknownValues2.Length != UnknownValues2Count)
            throw new InvalidOperationException(message: "Unexpected fixed avatar field count.");
    }
}
