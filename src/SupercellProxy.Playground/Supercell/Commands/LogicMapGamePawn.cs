using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

/// <summary>
/// Native map-game pawn structure encoded by the shared 1.72.84 helper at 0x10065c78c.
/// Semantic names for the stripped fields are not yet proven.
/// </summary>
public sealed record LogicMapGamePawn
{
    public LogicMapGamePawn(
        LogicLong? unknownLogicLong0,
        LogicLong? unknownLogicLong1,
        int unknown0,
        int unknown1,
        int unknown2,
        int unknown3,
        int unknown4,
        ReadOnlyMemory<int> unknownValues,
        ReadOnlyMemory<int> unknownGlobalIds,
        string? unknownString,
        LogicMapGamePawnNestedData? unknownNestedData,
        int unknownGlobalId,
        ReadOnlyMemory<LogicCommandDataReferenceVarIntPair> unknownPairs)
    {
        UnknownLogicLong0 = unknownLogicLong0;
        UnknownLogicLong1 = unknownLogicLong1;
        Unknown0 = unknown0;
        Unknown1 = unknown1;
        Unknown2 = unknown2;
        Unknown3 = unknown3;
        Unknown4 = unknown4;
        UnknownValues = unknownValues.ToArray();
        UnknownGlobalIds = unknownGlobalIds.ToArray();
        UnknownString = unknownString;
        UnknownNestedData = unknownNestedData;
        UnknownGlobalId = unknownGlobalId;
        UnknownPairs = unknownPairs.ToArray();
    }

    public LogicLong? UnknownLogicLong0 { get; }
    public LogicLong? UnknownLogicLong1 { get; }
    public int Unknown0 { get; }
    public int Unknown1 { get; }
    public int Unknown2 { get; }
    public int Unknown3 { get; }
    public int Unknown4 { get; }
    public ReadOnlyMemory<int> UnknownValues { get; }
    public ReadOnlyMemory<int> UnknownGlobalIds { get; }
    public string? UnknownString { get; }
    public LogicMapGamePawnNestedData? UnknownNestedData { get; }
    public int UnknownGlobalId { get; }
    public ReadOnlyMemory<LogicCommandDataReferenceVarIntPair> UnknownPairs { get; }

    internal static LogicMapGamePawn Decode(SupercellStream stream)
    {
        var unknownLogicLong0 = LogicMapGameWire.ReadOptionalLogicLong(stream);
        var unknownLogicLong1 = LogicMapGameWire.ReadOptionalLogicLong(stream);
        var unknown0 = stream.ReadVarInt();
        var unknown1 = stream.ReadVarInt();
        var unknown2 = stream.ReadVarInt();
        var unknown3 = stream.ReadVarInt();
        var unknown4 = stream.ReadVarInt();
        var unknownValues = LogicCommandVarIntArrayField.DecodeValues(stream.ReadVarInt(), stream);
        var unknownGlobalIds = LogicCommandDataReferenceArrayField.Decode(stream).GlobalIds;
        var unknownString = stream.ReadBoolean() ? stream.ReadString() : null;
        var unknownNestedData = stream.ReadBoolean() ? LogicMapGamePawnNestedData.Decode(stream) : null;
        var unknownGlobalId = stream.ReadVarInt();
        var unknownPairs = LogicCommandDataReferenceVarIntPairArrayField.Decode(stream).Values;

        return new LogicMapGamePawn(
            unknownLogicLong0,
            unknownLogicLong1,
            unknown0,
            unknown1,
            unknown2,
            unknown3,
            unknown4,
            unknownValues,
            unknownGlobalIds,
            unknownString,
            unknownNestedData,
            unknownGlobalId,
            unknownPairs);
    }

    internal void Encode(SupercellStream stream)
    {
        LogicMapGameWire.WriteOptionalLogicLong(stream, UnknownLogicLong0);
        LogicMapGameWire.WriteOptionalLogicLong(stream, UnknownLogicLong1);
        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteVarInt(Unknown2);
        stream.WriteVarInt(Unknown3);
        stream.WriteVarInt(Unknown4);
        new LogicCommandVarIntArrayField(UnknownValues).Encode(stream);
        new LogicCommandDataReferenceArrayField(UnknownGlobalIds).Encode(stream);
        stream.WriteBoolean(UnknownString is not null);

        if (UnknownString is not null)
            stream.WriteString(UnknownString);

        stream.WriteBoolean(UnknownNestedData is not null);
        UnknownNestedData?.Encode(stream);
        stream.WriteVarInt(UnknownGlobalId);
        new LogicCommandDataReferenceVarIntPairArrayField(UnknownPairs).Encode(stream);
    }
}
