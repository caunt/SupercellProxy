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
        ReadOnlyMemory<CommandDataReferenceVarIntPair> requiredGoods,
        ReadOnlyMemory<CommandDataReferenceVarIntPair>? optionalValues,
        bool unknown0,
        LongId? unknownLongId,
        int unknownGlobalId0,
        int unknownGlobalId1
    )
    {
        RequiredGoods = requiredGoods.ToArray();
        OptionalValues = optionalValues is null
            ? null
            : (ReadOnlyMemory<CommandDataReferenceVarIntPair>?)optionalValues.Value.ToArray();
        Unknown0 = unknown0;
        UnknownLongId = unknownLongId;
        UnknownGlobalId0 = unknownGlobalId0;
        UnknownGlobalId1 = unknownGlobalId1;
    }

    /// <summary>
    /// Gets the <c language="csharp">OptionalValues</c> value.
    /// </summary>
    public ReadOnlyMemory<CommandDataReferenceVarIntPair>? OptionalValues { get; }

    /// <summary>
    /// Gets the goods and quantities required to complete this dump task.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("Values")]
    public ReadOnlyMemory<CommandDataReferenceVarIntPair> RequiredGoods { get; }

    /// <summary>
    /// Gets the <c language="csharp">Unknown0</c> value.
    /// </summary>
    public bool Unknown0 { get; init; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownGlobalId0</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownGlobalId0")]
    public int UnknownGlobalId0 { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownGlobalId1</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownGlobalId1")]
    public int UnknownGlobalId1 { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownLongId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownLongId")]
    public LongId? UnknownLongId { get; init; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static MapGameDumpTaskStatePayload Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new MapGameDumpTaskStatePayload(
            MapGameFieldCodec.ReadDataReferenceVarIntPairs(stream),
            MapGameFieldCodec.ReadOptionalDataReferenceVarIntPairs(stream),
            stream.ReadBoolean(),
            MapGameFieldCodec.ReadOptionalLongId(stream),
            stream.ReadVarInt(),
            stream.ReadVarInt()
        );
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream)
    {
        MapGameFieldCodec.WriteDataReferenceVarIntPairs(stream, RequiredGoods.Span);
        MapGameFieldCodec.WriteOptionalDataReferenceVarIntPairs(stream, OptionalValues);
        stream.WriteBoolean(Unknown0);
        MapGameFieldCodec.WriteOptionalLongId(stream, UnknownLongId);
        stream.WriteVarInt(UnknownGlobalId0);
        stream.WriteVarInt(UnknownGlobalId1);
    }
}
