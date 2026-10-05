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
    public LongId AccountId { get; init; }

    /// Gets the country code used by age restrictions.
    public string? AgeCountryCode { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">AvatarVersion</c> value.
    /// </summary>
    public int AvatarVersion { get; init; }

    /// Gets the retained birth timestamp used by age restrictions.
    public int BirthTimestamp { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">InventoryValues</c> value.
    /// </summary>
    public int[][] InventoryValues { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">CanEditFarm</c> value.
    /// </summary>
    public bool CanEditFarm { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">DeprecatedInventoryDataCount</c> value.
    /// </summary>
    public int DeprecatedInventoryDataCount { get; init; }

    /// <summary>
    /// Gets or sets the in-game farm name.
    /// </summary>
    public string? FarmName { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">HomeId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("HomeId")]
    public LongId HomeId { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">InventoryMaps</c> value.
    /// </summary>
    public DataReferenceValue[][] InventoryMaps { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">InventoryUnknown0</c> value.
    /// </summary>
    public int InventoryUnknown0 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">RoadsideShop</c> value.
    /// </summary>
    public RoadsideShopEntry[] RoadsideShop { get; set; } = [];

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
    /// Gets or sets the <c language="csharp">MailEntries</c> value.
    /// </summary>
    public MailEntry[] MailEntries { get; set; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">MapGameId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("MapGameId")]
    public LongId? MapGameId { get; init; }

    /// <summary>
    /// Gets the Map Game Participants value.
    /// </summary>
    public MapGameParticipant[]? MapGameParticipants { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Neighborhood</c> value.
    /// </summary>
    public NeighborhoodData? Neighborhood { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownValues0</c> value.
    /// </summary>
    public int[] UnknownValues0 { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">BoatCrateHelpEntries</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownEntries0")]
    public BoatCrateHelpEntry[] BoatCrateHelpEntries { get; set; } = [];

    /// <summary>
    /// Gets elapsed seconds without real-money spending at the avatar's load boundary.
    /// </summary>
    public int SecondsWithoutSpending { get; init; }

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
    /// Gets or sets the <c language="csharp">Unknown4</c> value.
    /// </summary>
    public int Unknown4 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownBoolean1</c> value.
    /// </summary>
    public bool UnknownBoolean1 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownEntries1</c> value.
    /// </summary>
    public AvatarIdFlagEntry[] UnknownEntries1 { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">PickedPassengers</c> value.
    /// </summary>
    public PickedPassenger[] PickedPassengers { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownEntries2</c> value.
    /// </summary>
    public AvatarIdPairEntry[] UnknownEntries2 { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownLeagueValue</c> value.
    /// </summary>
    public int UnknownLeagueValue { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownEntries3</c> value.
    /// </summary>
    public AvatarIdPairEntry[] UnknownEntries3 { get; init; } = [];

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
    public LongId? UnknownOptionalId0 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownOptionalId1</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownOptionalId1")]
    public LongId? UnknownOptionalId1 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownOptionalId3</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownOptionalId3")]
    public LongId? UnknownOptionalId3 { get; init; }

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
        int unknown0 = stream.ReadVarInt();
        int unknown1 = stream.ReadVarInt();
        int avatarVersion = stream.ReadVarInt();
        int secondsWithoutSpending = stream.ReadVarInt();
        string? name = stream.ReadOptionalString();
        LongId homeId = stream.ReadLongId();
        LongId accountId = stream.ReadLongId();

        AvatarInventorySection inventory = DecodeInventory(stream);

        AvatarSocialSection social = DecodeSocialState(stream);

        AvatarProgressionSection progression = DecodeProgressionState(stream);

        return new ClientAvatar
        {
            Unknown0 = unknown0,
            Unknown1 = unknown1,
            AvatarVersion = avatarVersion,
            SecondsWithoutSpending = secondsWithoutSpending,
            FarmName = name,
            HomeId = homeId,
            AccountId = accountId,
            InventoryValues = inventory.Values,
            InventoryMaps = inventory.Maps,
            DeprecatedInventoryDataCount = inventory.DeprecatedDataCount,
            InventoryUnknown0 = inventory.Unknown0,
            RoadsideShop = social.RoadsideShop,
            Neighborhood = social.Neighborhood,
            MailEntries = social.MailEntries,
            UnknownValues0 = social.UnknownValues0,
            BoatCrateHelpEntries = social.BoatCrateHelpEntries,
            TrainStationReady = social.TrainStationReady,
            IsMuted = social.IsMuted,
            CanEditFarm = social.CanEditFarm,
            UnknownEntries1 = social.UnknownEntries1,
            PickedPassengers = social.PickedPassengers,
            UnknownEntries2 = social.UnknownEntries2,
            UnknownEntries3 = social.UnknownEntries3,
            UnknownNullableListCount = progression.UnknownNullableListCount,
            UnknownOptionalId0 = progression.UnknownOptionalId0,
            UnknownOptionalId1 = progression.UnknownOptionalId1,
            LeagueType = progression.LeagueType,
            UnknownLeagueValue = progression.UnknownLeagueValue,
            LeagueScore = progression.LeagueScore,
            UnknownValues1 = progression.UnknownValues1,
            UnknownManager0 = progression.UnknownManager0,
            UnknownManager1 = progression.UnknownManager1,
            UnknownValues2 = progression.UnknownValues2,
            MapGameId = progression.MapGameId,
            MapGameParticipants = progression.MapGameParticipants,
            UnknownOptionalId3 = progression.UnknownOptionalId3,
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

    private static AvatarInventorySection DecodeInventory(MessageStream stream)
    {
        int[][] values = new int[InventoryArrayCount][];

        for (int index = 0; index < values.Length; index++)
            values[index] = stream.ReadArray(static valueStream => valueStream.ReadVarInt());

        DataReferenceValue[][] maps = new DataReferenceValue[InventoryMapCount][];

        for (int index = 0; index < maps.Length; index++)
            maps[index] = stream.ReadArray(DataReferenceValue.Decode);

        int deprecatedDataCount = stream.ReadVarInt();

        return deprecatedDataCount is not 0
            ? throw new InvalidDataException(message: "The deprecated polymorphic inventory section is not implemented.")
            : new AvatarInventorySection(values, maps, deprecatedDataCount, stream.ReadVarInt());
    }

    private static AvatarProgressionSection DecodeProgressionState(MessageStream stream)
    {
        int unknownNullableListCount = stream.ReadVarInt();

        if (unknownNullableListCount > 0)
            throw new InvalidDataException(message: "The nullable polymorphic avatar section is not implemented.");

        LongId? unknownOptionalId0 = stream.ReadOptionalLongId();
        LongId? unknownOptionalId1 = stream.ReadOptionalLongId();
        int leagueType = stream.ReadVarInt();
        int unknownLeagueValue = stream.ReadVarInt();
        int leagueScore = stream.ReadVarInt();
        int[] unknownValues1 = stream.ReadVarIntArray(count: 11);
        AvatarCollectionSection unknownManager0 = AvatarCollectionSection.Decode(stream);
        AvatarStringSection unknownManager1 = AvatarStringSection.Decode(stream);
        int[] unknownValues2 = stream.ReadVarIntArray(count: 6);
        LongId? mapGameId = stream.ReadOptionalLongId();
        LongId? unknownOptionalId3 = stream.ReadOptionalLongId();
        int unknown4 = stream.ReadVarInt();

        MapGameParticipant[]? participants =
            mapGameId is not null && stream.ReadBoolean()
                ? stream.ReadArray(MapGameParticipant.Decode)
                : null;

        return new AvatarProgressionSection(
            unknownNullableListCount,
            unknownOptionalId0,
            unknownOptionalId1,
            leagueType,
            unknownLeagueValue,
            leagueScore,
            unknownValues1,
            unknownManager0,
            unknownManager1,
            unknownValues2,
            mapGameId,
            unknownOptionalId3,
            unknown4,
            participants,
            stream.ReadVarInt(),
            stream.ReadOptionalString(),
            stream.ReadBoolean(),
            stream.ReadOptionalString(),
            stream.ReadBoolean(),
            AvatarStateSection.Decode(stream),
            stream.ReadBoolean() ? AvatarSettings.Decode(stream) : null
        );
    }

    private static AvatarSocialSection DecodeSocialState(MessageStream stream)
    {
        return new AvatarSocialSection(
            stream.ReadArray(RoadsideShopEntry.Decode),
            stream.ReadBoolean() ? NeighborhoodData.Decode(stream) : null,
            stream.ReadArray(MailEntry.Decode),
            stream.ReadArray(static valueStream => valueStream.ReadVarInt()),
            stream.ReadArray(BoatCrateHelpEntry.Decode),
            stream.ReadBoolean(),
            stream.ReadBoolean(),
            stream.ReadBoolean(),
            stream.ReadArray(AvatarIdFlagEntry.Decode),
            stream.ReadArray(PickedPassenger.Decode),
            stream.ReadArray(AvatarIdPairEntry.Decode),
            stream.ReadArray(AvatarIdPairEntry.Decode)
        );
    }

    private void EncodeIdentityAndInventory(MessageStream stream)
    {
        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteVarInt(AvatarVersion);
        stream.WriteVarInt(SecondsWithoutSpending);
        stream.WriteOptionalString(FarmName);
        stream.WriteLongId(HomeId);
        stream.WriteLongId(AccountId);

        foreach (int[] values in InventoryValues)
            stream.WriteArray(values, static (valueStream, value) => valueStream.WriteVarInt(value));

        foreach (DataReferenceValue[] values in InventoryMaps)
            stream.WriteArray(values, static (valueStream, value) => value.Encode(valueStream));

        stream.WriteVarInt(DeprecatedInventoryDataCount);
        stream.WriteVarInt(InventoryUnknown0);
    }

    private void EncodeProgressionState(MessageStream stream)
    {
        stream.WriteVarInt(UnknownNullableListCount);
        stream.WriteOptionalLongId(UnknownOptionalId0);
        stream.WriteOptionalLongId(UnknownOptionalId1);
        stream.WriteVarInt(LeagueType);
        stream.WriteVarInt(UnknownLeagueValue);
        stream.WriteVarInt(LeagueScore);

        foreach (int value in UnknownValues1)
            stream.WriteVarInt(value);

        UnknownManager0.Encode(stream);
        UnknownManager1.Encode(stream);

        foreach (int value in UnknownValues2)
            stream.WriteVarInt(value);

        stream.WriteOptionalLongId(MapGameId);
        stream.WriteOptionalLongId(UnknownOptionalId3);
        stream.WriteVarInt(Unknown4);

        if (MapGameId is not null)
        {
            stream.WriteBoolean(MapGameParticipants is not null);

            if (MapGameParticipants is { } participants)
                stream.WriteArray(participants, static (output, value) => value.Encode(output));
        }

        stream.WriteVarInt(BirthTimestamp);
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
        stream.WriteArray(UnknownValues0, static (valueStream, value) => valueStream.WriteVarInt(value));
        stream.WriteArray(BoatCrateHelpEntries, static (valueStream, value) => value.Encode(valueStream));
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
