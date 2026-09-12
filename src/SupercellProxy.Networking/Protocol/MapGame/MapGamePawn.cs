using SupercellProxy.Networking.Protocol.CommandEncoding.CollectionFields;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MapGame;

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
        LongIdentifier? unknownLongIdentifier0,
        LongIdentifier? unknownLongIdentifier1,
        int unknown0,
        int unknown1,
        int unknown2,
        int unknown3,
        int unknown4,
        ReadOnlyMemory<int> unknownValues,
        ReadOnlyMemory<int> unknownGlobalIdentifiers,
        string? unknownString,
        MapGamePawnNestedData? unknownNestedData,
        int unknownGlobalIdentifier,
        ReadOnlyMemory<CommandDataReferenceVariableIntPair> unknownPairs
    )
    {
        UnknownLongIdentifier0 = unknownLongIdentifier0;
        UnknownLongIdentifier1 = unknownLongIdentifier1;
        Unknown0 = unknown0;
        Unknown1 = unknown1;
        Unknown2 = unknown2;
        Unknown3 = unknown3;
        Unknown4 = unknown4;
        UnknownValues = unknownValues.ToArray();
        UnknownGlobalIdentifiers = unknownGlobalIdentifiers.ToArray();
        UnknownString = unknownString;
        UnknownNestedData = unknownNestedData;
        UnknownGlobalIdentifier = unknownGlobalIdentifier;
        UnknownPairs = unknownPairs.ToArray();
    }

    /// <summary>
    /// Gets the <c language="csharp">Unknown0</c> value.
    /// </summary>
    public int Unknown0 { get; }

    /// <summary>
    /// Gets the <c language="csharp">Unknown1</c> value.
    /// </summary>
    public int Unknown1 { get; }

    /// <summary>
    /// Gets the <c language="csharp">Unknown2</c> value.
    /// </summary>
    public int Unknown2 { get; }

    /// <summary>
    /// Gets the <c language="csharp">Unknown3</c> value.
    /// </summary>
    public int Unknown3 { get; }

    /// <summary>
    /// Gets the <c language="csharp">Unknown4</c> value.
    /// </summary>
    public int Unknown4 { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownGlobalId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownGlobalId")]
    public int UnknownGlobalIdentifier { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownGlobalIds</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownGlobalIds")]
    public ReadOnlyMemory<int> UnknownGlobalIdentifiers { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownLongId0</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownLongId0")]
    public LongIdentifier? UnknownLongIdentifier0 { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownLongId1</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownLongId1")]
    public LongIdentifier? UnknownLongIdentifier1 { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownNestedData</c> value.
    /// </summary>
    public MapGamePawnNestedData? UnknownNestedData { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownPairs</c> value.
    /// </summary>
    public ReadOnlyMemory<CommandDataReferenceVariableIntPair> UnknownPairs { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownString</c> value.
    /// </summary>
    public string? UnknownString { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownValues</c> value.
    /// </summary>
    public ReadOnlyMemory<int> UnknownValues { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static MapGamePawn Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        LongIdentifier? unknownLongIdentifier0 = MapGameFieldCodec.ReadOptionalLongIdentifier(stream);
        LongIdentifier? unknownLongIdentifier1 = MapGameFieldCodec.ReadOptionalLongIdentifier(stream);
        int unknown0 = stream.ReadVariableInt();
        int unknown1 = stream.ReadVariableInt();
        int unknown2 = stream.ReadVariableInt();
        int unknown3 = stream.ReadVariableInt();
        int unknown4 = stream.ReadVariableInt();
        int[] unknownValues = CommandVariableIntArrayField.DecodeValues(stream.ReadVariableInt(), stream);
        ReadOnlyMemory<int> unknownGlobalIdentifiers = CommandDataReferenceArrayField.Decode(stream).GlobalIdentifiers;
        string? unknownString = stream.ReadBoolean() ? stream.ReadString() : null;
        MapGamePawnNestedData? unknownNestedData = stream.ReadBoolean() ? MapGamePawnNestedData.Decode(stream) : null;
        int unknownGlobalIdentifier = stream.ReadVariableInt();
        ReadOnlyMemory<CommandDataReferenceVariableIntPair> unknownPairs = CommandDataReferenceVariableIntPairArrayField.Decode(stream).Values;

        return new MapGamePawn(
            unknownLongIdentifier0,
            unknownLongIdentifier1,
            unknown0,
            unknown1,
            unknown2,
            unknown3,
            unknown4,
            unknownValues,
            unknownGlobalIdentifiers,
            unknownString,
            unknownNestedData,
            unknownGlobalIdentifier,
            unknownPairs
        );
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        MapGameFieldCodec.WriteOptionalLongIdentifier(stream, UnknownLongIdentifier0);
        MapGameFieldCodec.WriteOptionalLongIdentifier(stream, UnknownLongIdentifier1);
        stream.WriteVariableInt(Unknown0);
        stream.WriteVariableInt(Unknown1);
        stream.WriteVariableInt(Unknown2);
        stream.WriteVariableInt(Unknown3);
        stream.WriteVariableInt(Unknown4);
        new CommandVariableIntArrayField(UnknownValues).Encode(stream);
        new CommandDataReferenceArrayField(UnknownGlobalIdentifiers).Encode(stream);
        stream.WriteBoolean(UnknownString is not null);

        if (UnknownString is not null)
            stream.WriteString(UnknownString);

        stream.WriteBoolean(UnknownNestedData is not null);
        UnknownNestedData?.Encode(stream);
        stream.WriteVariableInt(UnknownGlobalIdentifier);
        new CommandDataReferenceVariableIntPairArrayField(UnknownPairs).Encode(stream);
    }
}
