using SupercellProxy.Networking.Protocol.CommandEncoding.CollectionFields;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MapGame.Tasks;

/// <summary>
/// Represents <c language="csharp">MapGameDumpTaskStatePayload</c>.
/// </summary>
public sealed record MapGameDumpTaskStatePayload : MapGameTaskStatePayload
{
    /// <summary>
    /// Initializes a new <see cref="MapGameDumpTaskStatePayload"/> instance.
    /// </summary>
    public MapGameDumpTaskStatePayload(
        ReadOnlyMemory<CommandDataReferenceVariableIntPair> values,
        ReadOnlyMemory<CommandDataReferenceVariableIntPair>? optionalValues,
        bool unknown0,
        LongIdentifier? unknownLongIdentifier,
        int unknownGlobalIdentifier0,
        int unknownGlobalIdentifier1
    )
    {
        Values = values.ToArray();
        OptionalValues = optionalValues is null
            ? null
            : (ReadOnlyMemory<CommandDataReferenceVariableIntPair>?)optionalValues.Value.ToArray();
        Unknown0 = unknown0;
        UnknownLongIdentifier = unknownLongIdentifier;
        UnknownGlobalIdentifier0 = unknownGlobalIdentifier0;
        UnknownGlobalIdentifier1 = unknownGlobalIdentifier1;
    }

    /// <summary>
    /// Gets the <c language="csharp">OptionalValues</c> value.
    /// </summary>
    public ReadOnlyMemory<CommandDataReferenceVariableIntPair>? OptionalValues { get; }

    /// <summary>
    /// Gets the <c language="csharp">Unknown0</c> value.
    /// </summary>
    public bool Unknown0 { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownGlobalId0</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownGlobalId0")]
    public int UnknownGlobalIdentifier0 { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownGlobalId1</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownGlobalId1")]
    public int UnknownGlobalIdentifier1 { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownLongId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownLongId")]
    public LongIdentifier? UnknownLongIdentifier { get; }

    /// <summary>
    /// Gets the <c language="csharp">Values</c> value.
    /// </summary>
    public ReadOnlyMemory<CommandDataReferenceVariableIntPair> Values { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static MapGameDumpTaskStatePayload Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new MapGameDumpTaskStatePayload(
            MapGameFieldCodec.ReadDataReferenceVariableIntPairs(stream),
            MapGameFieldCodec.ReadOptionalDataReferenceVariableIntPairs(stream),
            stream.ReadBoolean(),
            MapGameFieldCodec.ReadOptionalLongIdentifier(stream),
            stream.ReadVariableInt(),
            stream.ReadVariableInt()
        );
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream)
    {
        MapGameFieldCodec.WriteDataReferenceVariableIntPairs(stream, Values.Span);
        MapGameFieldCodec.WriteOptionalDataReferenceVariableIntPairs(stream, OptionalValues);
        stream.WriteBoolean(Unknown0);
        MapGameFieldCodec.WriteOptionalLongIdentifier(stream, UnknownLongIdentifier);
        stream.WriteVariableInt(UnknownGlobalIdentifier0);
        stream.WriteVariableInt(UnknownGlobalIdentifier1);
    }
}
