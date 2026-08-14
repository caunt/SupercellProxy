using SupercellProxy.Playground.Network.Streams;
using SupercellProxy.Playground.Supercell;
using System.Buffers.Binary;

namespace SupercellProxy.Playground.Network.Messages.Clientbound;

public record OtherHomeDataMessage : IMessage
{
    public LogicClientAvatar? HomeOwnerAvatar { get; init; }
    public int Unknown0 { get; init; }
    public LogicClientAvatar? ClientAvatar { get; init; }
    public Memory<byte>? UnknownCompressedJson { get; init; }
    public Memory<byte>? CompressedAvatarDataJson { get; init; }
    public Memory<byte>? CompressedHomeDataJson { get; init; }
    public Memory<byte> Fallback { get; init; }
    public Memory<byte> UnknownData
    {
        get
        {
            using var stream = SupercellStream.Create();
            WritePayload(stream);

            return stream.ToArray();
        }
    }

    public static OtherHomeDataMessage Create(MessageContainer container)
    {
        var data = container.Payload.ReadToEnd();

        if (!TryDecode(data, out var message))
            return new OtherHomeDataMessage { Fallback = data };

        return message;
    }

    private static bool TryDecode(Memory<byte> data, out OtherHomeDataMessage message)
    {
        try
        {
            using var stream = new SupercellStream(new MemoryStream(data.ToArray()));
            var homeOwnerAvatar = LogicClientAvatar.Decode(stream);
            var unknown0 = stream.ReadVarInt();
            var clientAvatar = LogicClientAvatar.Decode(stream);
            var unknownCompressedJson = ReadByteArray(stream);
            var compressedAvatarDataJson = ReadByteArray(stream);
            var compressedHomeDataJson = ReadByteArray(stream);

            if (stream.Position != stream.Length ||
                !IsCompressedJson(unknownCompressedJson) ||
                !IsCompressedJson(compressedAvatarDataJson) ||
                !IsCompressedJson(compressedHomeDataJson))
            {
                throw new InvalidDataException("Invalid compressed JSON tail.");
            }

            message = new OtherHomeDataMessage
            {
                HomeOwnerAvatar = homeOwnerAvatar,
                Unknown0 = unknown0,
                ClientAvatar = clientAvatar,
                UnknownCompressedJson = unknownCompressedJson,
                CompressedAvatarDataJson = compressedAvatarDataJson,
                CompressedHomeDataJson = compressedHomeDataJson
            };

            return true;
        }
        catch (Exception exception) when (exception is EndOfStreamException or InvalidDataException or ArgumentException or OverflowException)
        {
            message = default!;
            return false;
        }
    }

    private static Memory<byte>? ReadByteArray(SupercellStream stream)
    {
        var length = stream.ReadInt32();

        if (length is -1)
            return null;

        if (length < 0 || length > stream.Length - stream.Position)
            throw new InvalidDataException("Invalid byte array length.");

        return stream.ReadExactly(new byte[length]).ToArray();
    }

    private static bool IsCompressedJson(Memory<byte>? data)
    {
        if (data is null || data.Value.IsEmpty)
            return true;

        var span = data.Value.Span;
        if (span.Length < 6 || BinaryPrimitives.ReadInt32LittleEndian(span) < 0)
            return false;

        var zlibHeader = BinaryPrimitives.ReadUInt16BigEndian(span[sizeof(int)..]);

        return (zlibHeader & 0x0F00) == 0x0800 && zlibHeader % 31 == 0;
    }

    private static void WriteByteArray(SupercellStream stream, Memory<byte>? data)
    {
        if (data is null)
        {
            stream.WriteInt32(-1);
            return;
        }

        stream.WriteByteArray(data.Value.Span);
    }

    public MessageContainer ToContainer(ushort id, ushort version = 0)
    {
        using var stream = SupercellStream.Create();

        WritePayload(stream);

        return new MessageContainer(id, version, stream);
    }

    private void WritePayload(SupercellStream stream)
    {
        if (!Fallback.IsEmpty)
        {
            stream.Write(Fallback.Span);
            return;
        }

        (HomeOwnerAvatar ?? throw new InvalidOperationException($"{nameof(HomeOwnerAvatar)} is null.")).Encode(stream);
        stream.WriteVarInt(Unknown0);
        (ClientAvatar ?? throw new InvalidOperationException($"{nameof(ClientAvatar)} is null.")).Encode(stream);
        WriteByteArray(stream, UnknownCompressedJson);
        WriteByteArray(stream, CompressedAvatarDataJson);
        WriteByteArray(stream, CompressedHomeDataJson);
    }
}

public record LogicClientAvatar
{
    private const int InventoryArrayCount = 93;
    private const int InventoryMapCount = 3;
    private const int MaxCollectionCount = 0x10000;

    public int Unknown0 { get; init; }
    public int Unknown1 { get; init; }
    public int AvatarVersion { get; init; }
    public int Unknown3 { get; init; }
    public string? Name { get; init; }
    public LogicLong HomeId { get; init; }
    public LogicLong AccountId { get; init; }
    public int[][] InventoryValues { get; init; } = [];
    public DataReferenceValue[][] InventoryMaps { get; init; } = [];
    public int DeprecatedInventoryDataCount { get; init; }
    public int InventoryUnknown0 { get; init; }
    public RoadsideShopEntry[] RoadsideShop { get; init; } = [];
    public NeighborhoodData? Neighborhood { get; init; }
    public MailEntry[] MailEntries { get; init; } = [];
    public int[] UnknownValues0 { get; init; } = [];
    public AvatarEntryA[] UnknownEntries0 { get; init; } = [];
    public bool TrainStationReady { get; init; }
    public bool IsMuted { get; init; }
    public bool CanEditFarm { get; init; }
    public AvatarEntryB[] UnknownEntries1 { get; init; } = [];
    public PickedPassenger[] PickedPassengers { get; init; } = [];
    public AvatarEntryC[] UnknownEntries2 { get; init; } = [];
    public AvatarEntryC[] UnknownEntries3 { get; init; } = [];
    public int UnknownNullableListCount { get; init; }
    public LogicLong? UnknownOptionalId0 { get; init; }
    public LogicLong? UnknownOptionalId1 { get; init; }
    public int LeagueType { get; init; }
    public int UnknownLeagueValue { get; init; }
    public int LeagueScore { get; init; }
    public int[] UnknownValues1 { get; init; } = [];
    public AvatarManagerA UnknownManager0 { get; init; } = new();
    public AvatarStringManager UnknownManager1 { get; init; } = new();
    public int[] UnknownValues2 { get; init; } = [];
    public LogicLong? MapGameId { get; init; }
    public LogicLong? UnknownOptionalId3 { get; init; }
    public int Unknown4 { get; init; }
    public int Unknown5 { get; init; }
    public string? UnknownString0 { get; init; }
    public bool StorePromotionAllowed { get; init; }
    public string? UnknownString1 { get; init; }
    public bool UnknownBoolean1 { get; init; }
    public AvatarManagerB UnknownManager2 { get; init; } = new();
    public AvatarSettings? Settings { get; init; }

    internal static LogicClientAvatar Decode(SupercellStream stream)
    {
        var unknown0 = stream.ReadVarInt();
        var unknown1 = stream.ReadVarInt();
        var avatarVersion = stream.ReadVarInt();
        var unknown3 = stream.ReadVarInt();
        var name = ReadOptionalString(stream);
        var homeId = stream.ReadLogicLong();
        var accountId = stream.ReadLogicLong();
        var inventoryValues = new int[InventoryArrayCount][];

        for (var i = 0; i < inventoryValues.Length; i++)
            inventoryValues[i] = ReadArray(stream, static valueStream => valueStream.ReadVarInt());

        var inventoryMaps = new DataReferenceValue[InventoryMapCount][];

        for (var i = 0; i < inventoryMaps.Length; i++)
            inventoryMaps[i] = ReadArray(stream, DataReferenceValue.Decode);

        var deprecatedInventoryDataCount = stream.ReadVarInt();
        if (deprecatedInventoryDataCount is not 0)
            throw new InvalidDataException("The deprecated polymorphic inventory section is not implemented.");

        var inventoryUnknown0 = stream.ReadVarInt();
        var roadsideShop = ReadArray(stream, RoadsideShopEntry.Decode);
        var neighborhood = stream.ReadBoolean() ? NeighborhoodData.Decode(stream) : null;
        var mailEntries = ReadArray(stream, MailEntry.Decode);
        var unknownValues0 = ReadArray(stream, static valueStream => valueStream.ReadVarInt());
        var unknownEntries0 = ReadArray(stream, AvatarEntryA.Decode);
        var trainStationReady = stream.ReadBoolean();
        var isMuted = stream.ReadBoolean();
        var canEditFarm = stream.ReadBoolean();
        var unknownEntries1 = ReadArray(stream, AvatarEntryB.Decode);
        var pickedPassengers = ReadArray(stream, PickedPassenger.Decode);
        var unknownEntries2 = ReadArray(stream, AvatarEntryC.Decode);
        var unknownEntries3 = ReadArray(stream, AvatarEntryC.Decode);
        var unknownNullableListCount = stream.ReadVarInt();

        if (unknownNullableListCount > 0)
            throw new InvalidDataException("The nullable polymorphic avatar section is not implemented.");

        var unknownOptionalId0 = ReadOptionalLogicLong(stream);
        var unknownOptionalId1 = ReadOptionalLogicLong(stream);
        var leagueType = stream.ReadVarInt();
        var unknownLeagueValue = stream.ReadVarInt();
        var leagueScore = stream.ReadVarInt();
        var unknownValues1 = ReadValues(stream, 11);
        var unknownManager0 = AvatarManagerA.Decode(stream);
        var unknownManager1 = AvatarStringManager.Decode(stream);
        var unknownValues2 = ReadValues(stream, 6);
        var mapGameId = ReadOptionalLogicLong(stream);
        var unknownOptionalId3 = ReadOptionalLogicLong(stream);
        var unknown4 = stream.ReadVarInt();

        if (mapGameId is not null && stream.ReadBoolean())
            throw new InvalidDataException("The conditional avatar manager is not implemented.");

        var unknown5 = stream.ReadVarInt();
        var unknownString0 = ReadOptionalString(stream);
        var storePromotionAllowed = stream.ReadBoolean();
        var unknownString1 = ReadOptionalString(stream);
        var unknownBoolean1 = stream.ReadBoolean();
        var unknownManager2 = AvatarManagerB.Decode(stream);
        var settings = stream.ReadBoolean() ? AvatarSettings.Decode(stream) : null;

        return new LogicClientAvatar
        {
            Unknown0 = unknown0,
            Unknown1 = unknown1,
            AvatarVersion = avatarVersion,
            Unknown3 = unknown3,
            Name = name,
            HomeId = homeId,
            AccountId = accountId,
            InventoryValues = inventoryValues,
            InventoryMaps = inventoryMaps,
            DeprecatedInventoryDataCount = deprecatedInventoryDataCount,
            InventoryUnknown0 = inventoryUnknown0,
            RoadsideShop = roadsideShop,
            Neighborhood = neighborhood,
            MailEntries = mailEntries,
            UnknownValues0 = unknownValues0,
            UnknownEntries0 = unknownEntries0,
            TrainStationReady = trainStationReady,
            IsMuted = isMuted,
            CanEditFarm = canEditFarm,
            UnknownEntries1 = unknownEntries1,
            PickedPassengers = pickedPassengers,
            UnknownEntries2 = unknownEntries2,
            UnknownEntries3 = unknownEntries3,
            UnknownNullableListCount = unknownNullableListCount,
            UnknownOptionalId0 = unknownOptionalId0,
            UnknownOptionalId1 = unknownOptionalId1,
            LeagueType = leagueType,
            UnknownLeagueValue = unknownLeagueValue,
            LeagueScore = leagueScore,
            UnknownValues1 = unknownValues1,
            UnknownManager0 = unknownManager0,
            UnknownManager1 = unknownManager1,
            UnknownValues2 = unknownValues2,
            MapGameId = mapGameId,
            UnknownOptionalId3 = unknownOptionalId3,
            Unknown4 = unknown4,
            Unknown5 = unknown5,
            UnknownString0 = unknownString0,
            StorePromotionAllowed = storePromotionAllowed,
            UnknownString1 = unknownString1,
            UnknownBoolean1 = unknownBoolean1,
            UnknownManager2 = unknownManager2,
            Settings = settings
        };
    }

    internal void Encode(SupercellStream stream)
    {
        if (InventoryValues.Length != InventoryArrayCount || InventoryMaps.Length != InventoryMapCount)
            throw new InvalidOperationException("Unexpected inventory field count.");

        if (DeprecatedInventoryDataCount is not 0 || UnknownNullableListCount > 0)
            throw new InvalidOperationException("Cannot encode an unsupported avatar section.");

        if (UnknownValues1.Length != 11 || UnknownValues2.Length != 6)
            throw new InvalidOperationException("Unexpected fixed avatar field count.");

        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteVarInt(AvatarVersion);
        stream.WriteVarInt(Unknown3);
        stream.WriteOptionalString(Name);
        stream.WriteLogicLong(HomeId);
        stream.WriteLogicLong(AccountId);

        foreach (var values in InventoryValues)
            WriteArray(stream, values, static (valueStream, value) => valueStream.WriteVarInt(value));

        foreach (var values in InventoryMaps)
            WriteArray(stream, values, static (valueStream, value) => value.Encode(valueStream));

        stream.WriteVarInt(DeprecatedInventoryDataCount);
        stream.WriteVarInt(InventoryUnknown0);
        WriteArray(stream, RoadsideShop, static (valueStream, value) => value.Encode(valueStream));
        stream.WriteBoolean(Neighborhood is not null);
        Neighborhood?.Encode(stream);
        WriteArray(stream, MailEntries, static (valueStream, value) => value.Encode(valueStream));
        WriteArray(stream, UnknownValues0, static (valueStream, value) => valueStream.WriteVarInt(value));
        WriteArray(stream, UnknownEntries0, static (valueStream, value) => value.Encode(valueStream));
        stream.WriteBoolean(TrainStationReady);
        stream.WriteBoolean(IsMuted);
        stream.WriteBoolean(CanEditFarm);
        WriteArray(stream, UnknownEntries1, static (valueStream, value) => value.Encode(valueStream));
        WriteArray(stream, PickedPassengers, static (valueStream, value) => value.Encode(valueStream));
        WriteArray(stream, UnknownEntries2, static (valueStream, value) => value.Encode(valueStream));
        WriteArray(stream, UnknownEntries3, static (valueStream, value) => value.Encode(valueStream));
        stream.WriteVarInt(UnknownNullableListCount);
        WriteOptionalLogicLong(stream, UnknownOptionalId0);
        WriteOptionalLogicLong(stream, UnknownOptionalId1);
        stream.WriteVarInt(LeagueType);
        stream.WriteVarInt(UnknownLeagueValue);
        stream.WriteVarInt(LeagueScore);

        foreach (var value in UnknownValues1)
            stream.WriteVarInt(value);

        UnknownManager0.Encode(stream);
        UnknownManager1.Encode(stream);

        foreach (var value in UnknownValues2)
            stream.WriteVarInt(value);

        WriteOptionalLogicLong(stream, MapGameId);
        WriteOptionalLogicLong(stream, UnknownOptionalId3);
        stream.WriteVarInt(Unknown4);

        if (MapGameId is not null)
            stream.WriteBoolean(false);

        stream.WriteVarInt(Unknown5);
        stream.WriteOptionalString(UnknownString0);
        stream.WriteBoolean(StorePromotionAllowed);
        stream.WriteOptionalString(UnknownString1);
        stream.WriteBoolean(UnknownBoolean1);
        UnknownManager2.Encode(stream);
        stream.WriteBoolean(Settings is not null);
        Settings?.Encode(stream);
    }

    internal static T[] ReadArray<T>(SupercellStream stream, Func<SupercellStream, T> decode)
    {
        var count = stream.ReadVarInt();

        if (count < 0 || count > MaxCollectionCount)
            throw new InvalidDataException("Invalid collection count.");

        var values = new T[count];

        for (var i = 0; i < values.Length; i++)
            values[i] = decode(stream);

        return values;
    }

    internal static void WriteArray<T>(SupercellStream stream, T[] values, Action<SupercellStream, T> encode)
    {
        stream.WriteVarInt(values.Length);

        foreach (var value in values)
            encode(stream, value);
    }

    internal static int[] ReadValues(SupercellStream stream, int count)
    {
        var values = new int[count];

        for (var i = 0; i < values.Length; i++)
            values[i] = stream.ReadVarInt();

        return values;
    }

    internal static string? ReadOptionalString(SupercellStream stream)
    {
        var length = stream.ReadInt32();

        if (length is -1)
            return null;

        if (length < 0 || length > 900000 || length > stream.Length - stream.Position)
            throw new InvalidDataException("Invalid string length.");

        return System.Text.Encoding.UTF8.GetString(stream.ReadExactly(new byte[length]));
    }

    internal static Memory<byte>? ReadOptionalByteArray(SupercellStream stream)
    {
        var length = stream.ReadInt32();

        if (length is -1)
            return null;

        if (length < 0 || length > 900000 || length > stream.Length - stream.Position)
            throw new InvalidDataException("Invalid byte array length.");

        return stream.ReadExactly(new byte[length]).ToArray();
    }

    internal static void WriteOptionalByteArray(SupercellStream stream, Memory<byte>? value)
    {
        if (value is null)
        {
            stream.WriteInt32(-1);
            return;
        }

        stream.WriteByteArray(value.Value.Span);
    }

    internal static LogicLong? ReadOptionalLogicLong(SupercellStream stream)
    {
        return stream.ReadBoolean() ? stream.ReadLogicLong() : null;
    }

    internal static void WriteOptionalLogicLong(SupercellStream stream, LogicLong? value)
    {
        stream.WriteBoolean(value is not null);

        if (value is not null)
            stream.WriteLogicLong(value.Value);
    }
}

public record DataReferenceValue(int GlobalDataId, int Value)
{
    internal static DataReferenceValue Decode(SupercellStream stream)
    {
        return new DataReferenceValue(stream.ReadVarInt(), stream.ReadVarInt());
    }

    internal void Encode(SupercellStream stream)
    {
        stream.WriteVarInt(GlobalDataId);
        stream.WriteVarInt(Value);
    }
}

public record RoadsideShopEntry(LogicLong? UnknownId, bool Unknown0, int Unknown1, int Unknown2, int Unknown3)
{
    internal static RoadsideShopEntry Decode(SupercellStream stream)
    {
        return new RoadsideShopEntry(
            LogicClientAvatar.ReadOptionalLogicLong(stream),
            stream.ReadBoolean(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt());
    }

    internal void Encode(SupercellStream stream)
    {
        LogicClientAvatar.WriteOptionalLogicLong(stream, UnknownId);
        stream.WriteBoolean(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteVarInt(Unknown2);
        stream.WriteVarInt(Unknown3);
    }
}

public record NeighborhoodData(
    LogicLong NeighborhoodId,
    string? NeighborhoodName,
    int NeighborhoodRole,
    int BadgeUnknown0,
    int BadgeUnknown1,
    int BadgeUnknown2,
    int Unknown0,
    int Unknown1,
    int Unknown2)
{
    internal static NeighborhoodData Decode(SupercellStream stream)
    {
        return new NeighborhoodData(
            stream.ReadLogicLong(),
            LogicClientAvatar.ReadOptionalString(stream),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt());
    }

    internal void Encode(SupercellStream stream)
    {
        stream.WriteLogicLong(NeighborhoodId);
        stream.WriteOptionalString(NeighborhoodName);
        stream.WriteVarInt(NeighborhoodRole);
        stream.WriteVarInt(BadgeUnknown0);
        stream.WriteVarInt(BadgeUnknown1);
        stream.WriteVarInt(BadgeUnknown2);
        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteVarInt(Unknown2);
    }
}

public record MailEntry
{
    public int Unknown0 { get; init; }
    public int Unknown1 { get; init; }
    public long Unknown2 { get; init; }
    public string? SenderAvatarName { get; init; }
    public int Unknown3 { get; init; }
    public int Unknown4 { get; init; }
    public int Unknown5 { get; init; }
    public int Unknown6 { get; init; }
    public int Unknown7 { get; init; }
    public string? Subject { get; init; }
    public string? Body { get; init; }
    public int Unknown8 { get; init; }
    public string? FacebookId { get; init; }
    public string? GameCenterId { get; init; }
    public int Unknown9 { get; init; }
    public int Unknown10 { get; init; }
    public int Unknown11 { get; init; }
    public int Unknown12 { get; init; }
    public int Unknown13 { get; init; }
    public int Unknown14 { get; init; }
    public string? CustomSubject { get; init; }
    public string? CustomBody { get; init; }
    public int Unknown15 { get; init; }
    public int Unknown16 { get; init; }
    public string? UnknownString0 { get; init; }
    public string? UnknownString1 { get; init; }

    internal static MailEntry Decode(SupercellStream stream)
    {
        return new MailEntry
        {
            Unknown0 = stream.ReadVarInt(),
            Unknown1 = stream.ReadVarInt(),
            Unknown2 = stream.ReadInt64(),
            SenderAvatarName = LogicClientAvatar.ReadOptionalString(stream),
            Unknown3 = stream.ReadVarInt(),
            Unknown4 = stream.ReadVarInt(),
            Unknown5 = stream.ReadVarInt(),
            Unknown6 = stream.ReadVarInt(),
            Unknown7 = stream.ReadVarInt(),
            Subject = LogicClientAvatar.ReadOptionalString(stream),
            Body = LogicClientAvatar.ReadOptionalString(stream),
            Unknown8 = stream.ReadVarInt(),
            FacebookId = LogicClientAvatar.ReadOptionalString(stream),
            GameCenterId = LogicClientAvatar.ReadOptionalString(stream),
            Unknown9 = stream.ReadVarInt(),
            Unknown10 = stream.ReadVarInt(),
            Unknown11 = stream.ReadVarInt(),
            Unknown12 = stream.ReadVarInt(),
            Unknown13 = stream.ReadVarInt(),
            Unknown14 = stream.ReadVarInt(),
            CustomSubject = LogicClientAvatar.ReadOptionalString(stream),
            CustomBody = LogicClientAvatar.ReadOptionalString(stream),
            Unknown15 = stream.ReadVarInt(),
            Unknown16 = stream.ReadVarInt(),
            UnknownString0 = LogicClientAvatar.ReadOptionalString(stream),
            UnknownString1 = LogicClientAvatar.ReadOptionalString(stream)
        };
    }

    internal void Encode(SupercellStream stream)
    {
        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteInt64(Unknown2);
        stream.WriteOptionalString(SenderAvatarName);
        stream.WriteVarInt(Unknown3);
        stream.WriteVarInt(Unknown4);
        stream.WriteVarInt(Unknown5);
        stream.WriteVarInt(Unknown6);
        stream.WriteVarInt(Unknown7);
        stream.WriteOptionalString(Subject);
        stream.WriteOptionalString(Body);
        stream.WriteVarInt(Unknown8);
        stream.WriteOptionalString(FacebookId);
        stream.WriteOptionalString(GameCenterId);
        stream.WriteVarInt(Unknown9);
        stream.WriteVarInt(Unknown10);
        stream.WriteVarInt(Unknown11);
        stream.WriteVarInt(Unknown12);
        stream.WriteVarInt(Unknown13);
        stream.WriteVarInt(Unknown14);
        stream.WriteOptionalString(CustomSubject);
        stream.WriteOptionalString(CustomBody);
        stream.WriteVarInt(Unknown15);
        stream.WriteVarInt(Unknown16);
        stream.WriteOptionalString(UnknownString0);
        stream.WriteOptionalString(UnknownString1);
    }
}

public record AvatarEntryA(int Unknown0, int Unknown1, int Unknown2, LogicLong? UnknownId)
{
    internal static AvatarEntryA Decode(SupercellStream stream)
    {
        return new AvatarEntryA(
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            LogicClientAvatar.ReadOptionalLogicLong(stream));
    }

    internal void Encode(SupercellStream stream)
    {
        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteVarInt(Unknown2);
        LogicClientAvatar.WriteOptionalLogicLong(stream, UnknownId);
    }
}

public record AvatarEntryB(LogicLong? UnknownId, int Unknown0, bool Unknown1)
{
    internal static AvatarEntryB Decode(SupercellStream stream)
    {
        return new AvatarEntryB(
            LogicClientAvatar.ReadOptionalLogicLong(stream),
            stream.ReadVarInt(),
            stream.ReadBoolean());
    }

    internal void Encode(SupercellStream stream)
    {
        LogicClientAvatar.WriteOptionalLogicLong(stream, UnknownId);
        stream.WriteVarInt(Unknown0);
        stream.WriteBoolean(Unknown1);
    }
}

public record PickedPassenger(
    int Unknown0,
    int Unknown1,
    int Unknown2,
    LogicLong UnknownId0,
    LogicLong UnknownId1,
    string? UnknownString0)
{
    internal static PickedPassenger Decode(SupercellStream stream)
    {
        return new PickedPassenger(
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadLogicLong(),
            stream.ReadLogicLong(),
            LogicClientAvatar.ReadOptionalString(stream));
    }

    internal void Encode(SupercellStream stream)
    {
        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteVarInt(Unknown2);
        stream.WriteLogicLong(UnknownId0);
        stream.WriteLogicLong(UnknownId1);
        stream.WriteOptionalString(UnknownString0);
    }
}

public record AvatarEntryC(LogicLong? UnknownId, int Unknown0, int Unknown1, bool Unknown2)
{
    internal static AvatarEntryC Decode(SupercellStream stream)
    {
        return new AvatarEntryC(
            LogicClientAvatar.ReadOptionalLogicLong(stream),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadBoolean());
    }

    internal void Encode(SupercellStream stream)
    {
        LogicClientAvatar.WriteOptionalLogicLong(stream, UnknownId);
        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteBoolean(Unknown2);
    }
}

public record AvatarManagerA
{
    public int Version { get; init; }
    public AvatarManagerAOptional? Optional { get; init; }
    public VarIntInt32Entry[] FixedValues { get; init; } = [];
    public VarIntPair[] Pairs { get; init; } = [];
    public int[] UnknownValues0 { get; init; } = [];
    public int[] UnknownValues1 { get; init; } = [];
    public VarIntStringEntry[] Strings { get; init; } = [];
    public AvatarManagerAItem[] UnknownEntries0 { get; init; } = [];
    public AvatarManagerAItem[] UnknownEntries1 { get; init; } = [];
    public VarIntTriple[] Triples { get; init; } = [];

    internal static AvatarManagerA Decode(SupercellStream stream)
    {
        var version = stream.ReadVarInt();
        var optional = stream.ReadBoolean() ? AvatarManagerAOptional.Decode(stream) : null;

        return new AvatarManagerA
        {
            Version = version,
            Optional = optional,
            FixedValues = LogicClientAvatar.ReadArray(stream, VarIntInt32Entry.Decode),
            Pairs = LogicClientAvatar.ReadArray(stream, VarIntPair.Decode),
            UnknownValues0 = LogicClientAvatar.ReadArray(stream, static valueStream => valueStream.ReadVarInt()),
            UnknownValues1 = LogicClientAvatar.ReadArray(stream, static valueStream => valueStream.ReadVarInt()),
            Strings = LogicClientAvatar.ReadArray(stream, VarIntStringEntry.Decode),
            UnknownEntries0 = LogicClientAvatar.ReadArray(stream, AvatarManagerAItem.Decode),
            UnknownEntries1 = LogicClientAvatar.ReadArray(stream, AvatarManagerAItem.Decode),
            Triples = LogicClientAvatar.ReadArray(stream, VarIntTriple.Decode)
        };
    }

    internal void Encode(SupercellStream stream)
    {
        stream.WriteVarInt(Version);
        stream.WriteBoolean(Optional is not null);
        Optional?.Encode(stream);
        LogicClientAvatar.WriteArray(stream, FixedValues, static (valueStream, value) => value.Encode(valueStream));
        LogicClientAvatar.WriteArray(stream, Pairs, static (valueStream, value) => value.Encode(valueStream));
        LogicClientAvatar.WriteArray(stream, UnknownValues0, static (valueStream, value) => valueStream.WriteVarInt(value));
        LogicClientAvatar.WriteArray(stream, UnknownValues1, static (valueStream, value) => valueStream.WriteVarInt(value));
        LogicClientAvatar.WriteArray(stream, Strings, static (valueStream, value) => value.Encode(valueStream));
        LogicClientAvatar.WriteArray(stream, UnknownEntries0, static (valueStream, value) => value.Encode(valueStream));
        LogicClientAvatar.WriteArray(stream, UnknownEntries1, static (valueStream, value) => value.Encode(valueStream));
        LogicClientAvatar.WriteArray(stream, Triples, static (valueStream, value) => value.Encode(valueStream));
    }
}

public record AvatarManagerAOptional(int Unknown0, AvatarManagerASpecial[] Entries)
{
    internal static AvatarManagerAOptional Decode(SupercellStream stream)
    {
        return new AvatarManagerAOptional(
            stream.ReadVarInt(),
            LogicClientAvatar.ReadArray(stream, AvatarManagerASpecial.Decode));
    }

    internal void Encode(SupercellStream stream)
    {
        stream.WriteVarInt(Unknown0);
        LogicClientAvatar.WriteArray(stream, Entries, static (valueStream, value) => value.Encode(valueStream));
    }
}

public record AvatarManagerASpecial
{
    public bool UsesCompressedData { get; init; }
    public string? Text { get; init; }
    public Memory<byte>? CompressedData { get; init; }
    public int Unknown0 { get; init; }
    public int Unknown1 { get; init; }
    public string? UnknownString0 { get; init; }
    public int[] UnknownValues { get; init; } = [];
    public string? UnknownString1 { get; init; }

    internal static AvatarManagerASpecial Decode(SupercellStream stream)
    {
        var usesCompressedData = stream.ReadBoolean();

        return new AvatarManagerASpecial
        {
            UsesCompressedData = usesCompressedData,
            Text = usesCompressedData ? null : LogicClientAvatar.ReadOptionalString(stream),
            CompressedData = usesCompressedData ? LogicClientAvatar.ReadOptionalByteArray(stream) : null,
            Unknown0 = stream.ReadVarInt(),
            Unknown1 = stream.ReadVarInt(),
            UnknownString0 = LogicClientAvatar.ReadOptionalString(stream),
            UnknownValues = LogicClientAvatar.ReadValues(stream, 11),
            UnknownString1 = LogicClientAvatar.ReadOptionalString(stream)
        };
    }

    internal void Encode(SupercellStream stream)
    {
        if (UnknownValues.Length != 11)
            throw new InvalidOperationException("Unexpected manager field count.");

        stream.WriteBoolean(UsesCompressedData);

        if (UsesCompressedData)
            LogicClientAvatar.WriteOptionalByteArray(stream, CompressedData);
        else
            stream.WriteOptionalString(Text);

        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteOptionalString(UnknownString0);

        foreach (var value in UnknownValues)
            stream.WriteVarInt(value);

        stream.WriteOptionalString(UnknownString1);
    }
}

public record AvatarManagerAItem(int Unknown0, int Kind, int Unknown1, int? KindValue, int Unknown2)
{
    internal static AvatarManagerAItem Decode(SupercellStream stream)
    {
        var unknown0 = stream.ReadVarInt();
        var kind = stream.ReadVarInt();
        var unknown1 = stream.ReadVarInt();
        int? kindValue = kind is 1 ? stream.ReadVarInt() : null;

        return new AvatarManagerAItem(unknown0, kind, unknown1, kindValue, stream.ReadVarInt());
    }

    internal void Encode(SupercellStream stream)
    {
        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Kind);
        stream.WriteVarInt(Unknown1);

        if (Kind is 1)
            stream.WriteVarInt(KindValue ?? throw new InvalidOperationException($"{nameof(KindValue)} is null."));

        stream.WriteVarInt(Unknown2);
    }
}

public record AvatarStringManager(string? UnknownString0, string? UnknownString1, string? UnknownString2)
{
    public AvatarStringManager() : this(null, null, null)
    {
    }

    internal static AvatarStringManager Decode(SupercellStream stream)
    {
        return new AvatarStringManager(
            LogicClientAvatar.ReadOptionalString(stream),
            LogicClientAvatar.ReadOptionalString(stream),
            LogicClientAvatar.ReadOptionalString(stream));
    }

    internal void Encode(SupercellStream stream)
    {
        stream.WriteOptionalString(UnknownString0);
        stream.WriteOptionalString(UnknownString1);
        stream.WriteOptionalString(UnknownString2);
    }
}

public record AvatarManagerB
{
    public int Version { get; init; }
    public AvatarManagerBEntry[] Entries { get; init; } = [];
    public AvatarManagerBState State { get; init; } = new(0, 0, 0, 0);
    public AvatarManagerBMapEntry[] UnknownEntries0 { get; init; } = [];
    public AvatarManagerBMapEntry[] UnknownEntries1 { get; init; } = [];

    internal static AvatarManagerB Decode(SupercellStream stream)
    {
        var version = stream.ReadVarInt();

        if (version <= 0)
            return new AvatarManagerB { Version = version };

        return new AvatarManagerB
        {
            Version = version,
            Entries = LogicClientAvatar.ReadArray(stream, AvatarManagerBEntry.Decode),
            State = AvatarManagerBState.Decode(stream),
            UnknownEntries0 = LogicClientAvatar.ReadArray(stream, AvatarManagerBMapEntry.Decode),
            UnknownEntries1 = LogicClientAvatar.ReadArray(stream, AvatarManagerBMapEntry.Decode)
        };
    }

    internal void Encode(SupercellStream stream)
    {
        stream.WriteVarInt(Version);

        if (Version <= 0)
            return;

        LogicClientAvatar.WriteArray(stream, Entries, static (valueStream, value) => value.Encode(valueStream));
        State.Encode(stream);
        LogicClientAvatar.WriteArray(stream, UnknownEntries0, static (valueStream, value) => value.Encode(valueStream));
        LogicClientAvatar.WriteArray(stream, UnknownEntries1, static (valueStream, value) => value.Encode(valueStream));
    }
}

public record AvatarManagerBEntry(long Unknown0, int Unknown1, int Unknown2, Int32VarIntEntry[] Values)
{
    internal static AvatarManagerBEntry Decode(SupercellStream stream)
    {
        return new AvatarManagerBEntry(
            stream.ReadInt64(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            LogicClientAvatar.ReadArray(stream, Int32VarIntEntry.Decode));
    }

    internal void Encode(SupercellStream stream)
    {
        stream.WriteInt64(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteVarInt(Unknown2);
        LogicClientAvatar.WriteArray(stream, Values, static (valueStream, value) => value.Encode(valueStream));
    }
}

public record AvatarManagerBState(int Unknown0, int Unknown1, int Unknown2, long Unknown3)
{
    internal static AvatarManagerBState Decode(SupercellStream stream)
    {
        return new AvatarManagerBState(stream.ReadVarInt(), stream.ReadVarInt(), stream.ReadVarInt(), stream.ReadInt64());
    }

    internal void Encode(SupercellStream stream)
    {
        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteVarInt(Unknown2);
        stream.WriteInt64(Unknown3);
    }
}

public record AvatarManagerBMapEntry(int Key, AvatarManagerBState State)
{
    internal static AvatarManagerBMapEntry Decode(SupercellStream stream)
    {
        return new AvatarManagerBMapEntry(stream.ReadVarInt(), AvatarManagerBState.Decode(stream));
    }

    internal void Encode(SupercellStream stream)
    {
        stream.WriteVarInt(Key);
        State.Encode(stream);
    }
}

public record AvatarSettings(int Version, AvatarSetting[] Entries, bool Unknown0)
{
    internal static AvatarSettings Decode(SupercellStream stream)
    {
        var version = stream.ReadVarInt();
        var entries = LogicClientAvatar.ReadArray(stream, AvatarSetting.Decode);

        return new AvatarSettings(version, entries, stream.ReadBoolean());
    }

    internal void Encode(SupercellStream stream)
    {
        stream.WriteVarInt(Version);
        LogicClientAvatar.WriteArray(stream, Entries, static (valueStream, value) => value.Encode(valueStream));
        stream.WriteBoolean(Unknown0);
    }
}

public record AvatarSetting(bool Enabled, int Value)
{
    internal static AvatarSetting Decode(SupercellStream stream)
    {
        return new AvatarSetting(stream.ReadBoolean(), stream.ReadVarInt());
    }

    internal void Encode(SupercellStream stream)
    {
        stream.WriteBoolean(Enabled);
        stream.WriteVarInt(Value);
    }
}

public record VarIntInt32Entry(int Key, int Value)
{
    internal static VarIntInt32Entry Decode(SupercellStream stream)
    {
        return new VarIntInt32Entry(stream.ReadVarInt(), stream.ReadInt32());
    }

    internal void Encode(SupercellStream stream)
    {
        stream.WriteVarInt(Key);
        stream.WriteInt32(Value);
    }
}

public record VarIntPair(int Key, int Value)
{
    internal static VarIntPair Decode(SupercellStream stream)
    {
        return new VarIntPair(stream.ReadVarInt(), stream.ReadVarInt());
    }

    internal void Encode(SupercellStream stream)
    {
        stream.WriteVarInt(Key);
        stream.WriteVarInt(Value);
    }
}

public record VarIntStringEntry(int Key, string? Value)
{
    internal static VarIntStringEntry Decode(SupercellStream stream)
    {
        return new VarIntStringEntry(stream.ReadVarInt(), LogicClientAvatar.ReadOptionalString(stream));
    }

    internal void Encode(SupercellStream stream)
    {
        stream.WriteVarInt(Key);
        stream.WriteOptionalString(Value);
    }
}

public record VarIntTriple(int Unknown0, int Unknown1, int Unknown2)
{
    internal static VarIntTriple Decode(SupercellStream stream)
    {
        return new VarIntTriple(stream.ReadVarInt(), stream.ReadVarInt(), stream.ReadVarInt());
    }

    internal void Encode(SupercellStream stream)
    {
        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteVarInt(Unknown2);
    }
}

public record Int32VarIntEntry(int Key, int Value)
{
    internal static Int32VarIntEntry Decode(SupercellStream stream)
    {
        return new Int32VarIntEntry(stream.ReadInt32(), stream.ReadVarInt());
    }

    internal void Encode(SupercellStream stream)
    {
        stream.WriteInt32(Key);
        stream.WriteVarInt(Value);
    }
}
