using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents <c language="csharp">ClientAvatar</c>.
/// </summary>
internal sealed record ClientAvatar
{
    private const int InventoryArrayCount = 93;
    private const int InventoryMapCount = 3;

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown0</c> value.
    /// </summary>
    public int Unknown0 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown1</c> value.
    /// </summary>
    public int Unknown1 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">AvatarVersion</c> value.
    /// </summary>
    public int AvatarVersion { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown3</c> value.
    /// </summary>
    public int Unknown3 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Name</c> value.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">HomeId</c> value.
    /// </summary>
    public LongId HomeId { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">AccountId</c> value.
    /// </summary>
    public LongId AccountId { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">InventoryValues</c> value.
    /// </summary>
    public int[][] InventoryValues { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">InventoryMaps</c> value.
    /// </summary>
    public DataReferenceValue[][] InventoryMaps { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">DeprecatedInventoryDataCount</c> value.
    /// </summary>
    public int DeprecatedInventoryDataCount { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">InventoryUnknown0</c> value.
    /// </summary>
    public int InventoryUnknown0 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">RoadsideShop</c> value.
    /// </summary>
    public RoadsideShopEntry[] RoadsideShop { get; init; } = [];

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
    /// Gets or sets the <c language="csharp">UnknownEntries0</c> value.
    /// </summary>
    public AvatarEntryA[] UnknownEntries0 { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">TrainStationReady</c> value.
    /// </summary>
    public bool TrainStationReady { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">IsMuted</c> value.
    /// </summary>
    public bool IsMuted { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">CanEditFarm</c> value.
    /// </summary>
    public bool CanEditFarm { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownEntries1</c> value.
    /// </summary>
    public AvatarEntryB[] UnknownEntries1 { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">PickedPassengers</c> value.
    /// </summary>
    public PickedPassenger[] PickedPassengers { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownEntries2</c> value.
    /// </summary>
    public AvatarEntryC[] UnknownEntries2 { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownEntries3</c> value.
    /// </summary>
    public AvatarEntryC[] UnknownEntries3 { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownNullableListCount</c> value.
    /// </summary>
    public int UnknownNullableListCount { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownOptionalId0</c> value.
    /// </summary>
    public LongId? UnknownOptionalId0 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownOptionalId1</c> value.
    /// </summary>
    public LongId? UnknownOptionalId1 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">LeagueType</c> value.
    /// </summary>
    public int LeagueType { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownLeagueValue</c> value.
    /// </summary>
    public int UnknownLeagueValue { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">LeagueScore</c> value.
    /// </summary>
    public int LeagueScore { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownValues1</c> value.
    /// </summary>
    public int[] UnknownValues1 { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownManager0</c> value.
    /// </summary>
    public AvatarManagerA UnknownManager0 { get; init; } = new();

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownManager1</c> value.
    /// </summary>
    public AvatarStringManager UnknownManager1 { get; init; } = new();

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownValues2</c> value.
    /// </summary>
    public int[] UnknownValues2 { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">MapGameId</c> value.
    /// </summary>
    public LongId? MapGameId { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownOptionalId3</c> value.
    /// </summary>
    public LongId? UnknownOptionalId3 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown4</c> value.
    /// </summary>
    public int Unknown4 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown5</c> value.
    /// </summary>
    public int Unknown5 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownString0</c> value.
    /// </summary>
    public string? UnknownString0 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">StorePromotionAllowed</c> value.
    /// </summary>
    public bool StorePromotionAllowed { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownString1</c> value.
    /// </summary>
    public string? UnknownString1 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownBoolean1</c> value.
    /// </summary>
    public bool UnknownBoolean1 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownManager2</c> value.
    /// </summary>
    public AvatarManagerB UnknownManager2 { get; init; } = new();

    /// <summary>
    /// Gets or sets the <c language="csharp">Settings</c> value.
    /// </summary>
    public AvatarSettings? Settings { get; init; }

    internal static ClientAvatar Decode(MessageStream stream)
    {
        var unknown0 = stream.ReadVarInt();
        var unknown1 = stream.ReadVarInt();
        var avatarVersion = stream.ReadVarInt();
        var unknown3 = stream.ReadVarInt();
        var name = stream.ReadOptionalString();
        var homeId = stream.ReadLongId();
        var accountId = stream.ReadLongId();
        var inventory = DecodeInventory(stream);
        var social = DecodeSocialState(stream);
        var progression = DecodeProgressionState(stream);

        return new ClientAvatar
        {
            Unknown0 = unknown0,
            Unknown1 = unknown1,
            AvatarVersion = avatarVersion,
            Unknown3 = unknown3,
            Name = name,
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
            UnknownEntries0 = social.UnknownEntries0,
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
            UnknownOptionalId3 = progression.UnknownOptionalId3,
            Unknown4 = progression.Unknown4,
            Unknown5 = progression.Unknown5,
            UnknownString0 = progression.UnknownString0,
            StorePromotionAllowed = progression.StorePromotionAllowed,
            UnknownString1 = progression.UnknownString1,
            UnknownBoolean1 = progression.UnknownBoolean1,
            UnknownManager2 = progression.UnknownManager2,
            Settings = progression.Settings,
        };
    }

    private static (
        int[][] Values,
        DataReferenceValue[][] Maps,
        int DeprecatedDataCount,
        int Unknown0
    ) DecodeInventory(MessageStream stream)
    {
        var values = new int[InventoryArrayCount][];
        for (var i = 0; i < values.Length; i++)
            values[i] = stream.ReadArray(static valueStream => valueStream.ReadVarInt());

        var maps = new DataReferenceValue[InventoryMapCount][];
        for (var i = 0; i < maps.Length; i++)
            maps[i] = stream.ReadArray(DataReferenceValue.Decode);

        var deprecatedDataCount = stream.ReadVarInt();
        if (deprecatedDataCount is not 0)
            throw new InvalidDataException(
                "The deprecated polymorphic inventory section is not implemented."
            );

        return (values, maps, deprecatedDataCount, stream.ReadVarInt());
    }

    private static (
        RoadsideShopEntry[] RoadsideShop,
        NeighborhoodData? Neighborhood,
        MailEntry[] MailEntries,
        int[] UnknownValues0,
        AvatarEntryA[] UnknownEntries0,
        bool TrainStationReady,
        bool IsMuted,
        bool CanEditFarm,
        AvatarEntryB[] UnknownEntries1,
        PickedPassenger[] PickedPassengers,
        AvatarEntryC[] UnknownEntries2,
        AvatarEntryC[] UnknownEntries3
    ) DecodeSocialState(MessageStream stream)
    {
        return (
            stream.ReadArray(RoadsideShopEntry.Decode),
            stream.ReadBoolean() ? NeighborhoodData.Decode(stream) : null,
            stream.ReadArray(MailEntry.Decode),
            stream.ReadArray(static valueStream => valueStream.ReadVarInt()),
            stream.ReadArray(AvatarEntryA.Decode),
            stream.ReadBoolean(),
            stream.ReadBoolean(),
            stream.ReadBoolean(),
            stream.ReadArray(AvatarEntryB.Decode),
            stream.ReadArray(PickedPassenger.Decode),
            stream.ReadArray(AvatarEntryC.Decode),
            stream.ReadArray(AvatarEntryC.Decode)
        );
    }

    private static (
        int UnknownNullableListCount,
        LongId? UnknownOptionalId0,
        LongId? UnknownOptionalId1,
        int LeagueType,
        int UnknownLeagueValue,
        int LeagueScore,
        int[] UnknownValues1,
        AvatarManagerA UnknownManager0,
        AvatarStringManager UnknownManager1,
        int[] UnknownValues2,
        LongId? MapGameId,
        LongId? UnknownOptionalId3,
        int Unknown4,
        int Unknown5,
        string? UnknownString0,
        bool StorePromotionAllowed,
        string? UnknownString1,
        bool UnknownBoolean1,
        AvatarManagerB UnknownManager2,
        AvatarSettings? Settings
    ) DecodeProgressionState(MessageStream stream)
    {
        var unknownNullableListCount = stream.ReadVarInt();
        if (unknownNullableListCount > 0)
            throw new InvalidDataException(
                "The nullable polymorphic avatar section is not implemented."
            );

        var unknownOptionalId0 = stream.ReadOptionalLongId();
        var unknownOptionalId1 = stream.ReadOptionalLongId();
        var leagueType = stream.ReadVarInt();
        var unknownLeagueValue = stream.ReadVarInt();
        var leagueScore = stream.ReadVarInt();
        var unknownValues1 = stream.ReadVarIntArray(11);
        var unknownManager0 = AvatarManagerA.Decode(stream);
        var unknownManager1 = AvatarStringManager.Decode(stream);
        var unknownValues2 = stream.ReadVarIntArray(6);
        var mapGameId = stream.ReadOptionalLongId();
        var unknownOptionalId3 = stream.ReadOptionalLongId();
        var unknown4 = stream.ReadVarInt();
        if (mapGameId is not null && stream.ReadBoolean())
            throw new InvalidDataException("The conditional avatar manager is not implemented.");

        return (
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
            stream.ReadVarInt(),
            stream.ReadOptionalString(),
            stream.ReadBoolean(),
            stream.ReadOptionalString(),
            stream.ReadBoolean(),
            AvatarManagerB.Decode(stream),
            stream.ReadBoolean() ? AvatarSettings.Decode(stream) : null
        );
    }

    internal void Encode(MessageStream stream)
    {
        ValidateEncodableState();
        EncodeIdentityAndInventory(stream);
        EncodeSocialState(stream);
        EncodeProgressionState(stream);
    }

    private void ValidateEncodableState()
    {
        if (
            InventoryValues.Length != InventoryArrayCount
            || InventoryMaps.Length != InventoryMapCount
        )
            throw new InvalidOperationException("Unexpected inventory field count.");

        if (DeprecatedInventoryDataCount is not 0 || UnknownNullableListCount > 0)
            throw new InvalidOperationException("Cannot encode an unsupported avatar section.");

        if (UnknownValues1.Length is not 11 || UnknownValues2.Length is not 6)
            throw new InvalidOperationException("Unexpected fixed avatar field count.");
    }

    private void EncodeIdentityAndInventory(MessageStream stream)
    {
        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteVarInt(AvatarVersion);
        stream.WriteVarInt(Unknown3);
        stream.WriteOptionalString(Name);
        stream.WriteLongId(HomeId);
        stream.WriteLongId(AccountId);

        foreach (var values in InventoryValues)
            stream.WriteArray(
                values,
                static (valueStream, value) => valueStream.WriteVarInt(value)
            );

        foreach (var values in InventoryMaps)
            stream.WriteArray(values, static (valueStream, value) => value.Encode(valueStream));

        stream.WriteVarInt(DeprecatedInventoryDataCount);
        stream.WriteVarInt(InventoryUnknown0);
    }

    private void EncodeSocialState(MessageStream stream)
    {
        stream.WriteArray(RoadsideShop, static (valueStream, value) => value.Encode(valueStream));
        stream.WriteBoolean(Neighborhood is not null);
        Neighborhood?.Encode(stream);
        stream.WriteArray(MailEntries, static (valueStream, value) => value.Encode(valueStream));
        stream.WriteArray(
            UnknownValues0,
            static (valueStream, value) => valueStream.WriteVarInt(value)
        );
        stream.WriteArray(
            UnknownEntries0,
            static (valueStream, value) => value.Encode(valueStream)
        );
        stream.WriteBoolean(TrainStationReady);
        stream.WriteBoolean(IsMuted);
        stream.WriteBoolean(CanEditFarm);
        stream.WriteArray(
            UnknownEntries1,
            static (valueStream, value) => value.Encode(valueStream)
        );
        stream.WriteArray(
            PickedPassengers,
            static (valueStream, value) => value.Encode(valueStream)
        );
        stream.WriteArray(
            UnknownEntries2,
            static (valueStream, value) => value.Encode(valueStream)
        );
        stream.WriteArray(
            UnknownEntries3,
            static (valueStream, value) => value.Encode(valueStream)
        );
    }

    private void EncodeProgressionState(MessageStream stream)
    {
        stream.WriteVarInt(UnknownNullableListCount);
        stream.WriteOptionalLongId(UnknownOptionalId0);
        stream.WriteOptionalLongId(UnknownOptionalId1);
        stream.WriteVarInt(LeagueType);
        stream.WriteVarInt(UnknownLeagueValue);
        stream.WriteVarInt(LeagueScore);

        foreach (var value in UnknownValues1)
            stream.WriteVarInt(value);

        UnknownManager0.Encode(stream);
        UnknownManager1.Encode(stream);

        foreach (var value in UnknownValues2)
            stream.WriteVarInt(value);

        stream.WriteOptionalLongId(MapGameId);
        stream.WriteOptionalLongId(UnknownOptionalId3);
        stream.WriteVarInt(Unknown4);

        if (MapGameId is not null)
            stream.WriteBoolean(value: false);

        stream.WriteVarInt(Unknown5);
        stream.WriteOptionalString(UnknownString0);
        stream.WriteBoolean(StorePromotionAllowed);
        stream.WriteOptionalString(UnknownString1);
        stream.WriteBoolean(UnknownBoolean1);
        UnknownManager2.Encode(stream);
        stream.WriteBoolean(Settings is not null);
        Settings?.Encode(stream);
    }
}
