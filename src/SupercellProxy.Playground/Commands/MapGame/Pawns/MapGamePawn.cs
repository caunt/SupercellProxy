using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Native map-game pawn structure encoded by the shared 1.72.84 helper at 0x10065c78c.
/// Semantic names for the stripped fields are not yet proven.
/// </summary>
public sealed record MapGamePawn
{
    /// <summary>
    /// Initializes a new <see cref="MapGamePawn"/> instance.
    /// </summary>
    public MapGamePawn(
        LongId? unknownLongId0,
        LongId? unknownLongId1,
        int unknown0,
        int unknown1,
        int unknown2,
        int unknown3,
        int unknown4,
        ReadOnlyMemory<int> unknownValues,
        ReadOnlyMemory<int> unknownGlobalIds,
        string? unknownString,
        MapGamePawnNestedData? unknownNestedData,
        int unknownGlobalId,
        ReadOnlyMemory<CommandDataReferenceVarIntPair> unknownPairs
    )
    {
        UnknownLongId0 = unknownLongId0;
        UnknownLongId1 = unknownLongId1;
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

    /// <summary>
    /// Gets the <c>UnknownLongId0</c> value.
    /// </summary>
    public LongId? UnknownLongId0 { get; }

    /// <summary>
    /// Gets the <c>UnknownLongId1</c> value.
    /// </summary>
    public LongId? UnknownLongId1 { get; }

    /// <summary>
    /// Gets the <c>Unknown0</c> value.
    /// </summary>
    public int Unknown0 { get; }

    /// <summary>
    /// Gets the <c>Unknown1</c> value.
    /// </summary>
    public int Unknown1 { get; }

    /// <summary>
    /// Gets the <c>Unknown2</c> value.
    /// </summary>
    public int Unknown2 { get; }

    /// <summary>
    /// Gets the <c>Unknown3</c> value.
    /// </summary>
    public int Unknown3 { get; }

    /// <summary>
    /// Gets the <c>Unknown4</c> value.
    /// </summary>
    public int Unknown4 { get; }

    /// <summary>
    /// Gets the <c>UnknownValues</c> value.
    /// </summary>
    public ReadOnlyMemory<int> UnknownValues { get; }

    /// <summary>
    /// Gets the <c>UnknownGlobalIds</c> value.
    /// </summary>
    public ReadOnlyMemory<int> UnknownGlobalIds { get; }

    /// <summary>
    /// Gets the <c>UnknownString</c> value.
    /// </summary>
    public string? UnknownString { get; }

    /// <summary>
    /// Gets the <c>UnknownNestedData</c> value.
    /// </summary>
    public MapGamePawnNestedData? UnknownNestedData { get; }

    /// <summary>
    /// Gets the <c>UnknownGlobalId</c> value.
    /// </summary>
    public int UnknownGlobalId { get; }

    /// <summary>
    /// Gets the <c>UnknownPairs</c> value.
    /// </summary>
    public ReadOnlyMemory<CommandDataReferenceVarIntPair> UnknownPairs { get; }

    internal static MapGamePawn Decode(MessageStream stream)
    {
        var unknownLongId0 = MapGameWire.ReadOptionalLongId(stream);
        var unknownLongId1 = MapGameWire.ReadOptionalLongId(stream);
        var unknown0 = stream.ReadVarInt();
        var unknown1 = stream.ReadVarInt();
        var unknown2 = stream.ReadVarInt();
        var unknown3 = stream.ReadVarInt();
        var unknown4 = stream.ReadVarInt();
        var unknownValues = CommandVarIntArrayField.DecodeValues(stream.ReadVarInt(), stream);
        var unknownGlobalIds = CommandDataReferenceArrayField.Decode(stream).GlobalIds;
        var unknownString = stream.ReadBoolean() ? stream.ReadString() : null;
        var unknownNestedData = stream.ReadBoolean() ? MapGamePawnNestedData.Decode(stream) : null;
        var unknownGlobalId = stream.ReadVarInt();
        var unknownPairs = CommandDataReferenceVarIntPairArrayField.Decode(stream).Values;

        return new MapGamePawn(
            unknownLongId0,
            unknownLongId1,
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
            unknownPairs
        );
    }

    internal void Encode(MessageStream stream)
    {
        MapGameWire.WriteOptionalLongId(stream, UnknownLongId0);
        MapGameWire.WriteOptionalLongId(stream, UnknownLongId1);
        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteVarInt(Unknown2);
        stream.WriteVarInt(Unknown3);
        stream.WriteVarInt(Unknown4);
        new CommandVarIntArrayField(UnknownValues).Encode(stream);
        new CommandDataReferenceArrayField(UnknownGlobalIds).Encode(stream);
        stream.WriteBoolean(UnknownString is not null);

        if (UnknownString is not null)
            stream.WriteString(UnknownString);

        stream.WriteBoolean(UnknownNestedData is not null);
        UnknownNestedData?.Encode(stream);
        stream.WriteVarInt(UnknownGlobalId);
        new CommandDataReferenceVarIntPairArrayField(UnknownPairs).Encode(stream);
    }
}
