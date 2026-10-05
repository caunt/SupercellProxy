using SupercellProxy.Networking.Protocol.CommandEncoding.CollectionFields;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MapGame.Tasks;

/// <summary>
/// Represents <c language="csharp">MapGameChickenTaskStatePayload</c>.
/// </summary>
public sealed record MapGameChickenTaskStatePayload : MapGameTaskStatePayload
{
    /// <summary>
    /// Initializes a new <see cref="MapGameChickenTaskStatePayload"/> instance.
    /// </summary>
    public MapGameChickenTaskStatePayload(
        int unknown0,
        bool unknownBoolean0,
        LongId? unknownLongId,
        ReadOnlyMemory<CommandDataReferenceVarIntPair>? optionalValues,
        ReadOnlyMemory<LongId> logicLongs
    )
    {
        Unknown0 = unknown0;
        UnknownBoolean0 = unknownBoolean0;
        UnknownLongId = unknownLongId;
        OptionalValues = optionalValues is null
            ? null
            : (ReadOnlyMemory<CommandDataReferenceVarIntPair>?)optionalValues.Value.ToArray();
        LongIds = logicLongs.ToArray();
    }

    /// <summary>
    /// Gets the <c language="csharp">LongIds</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("LongIds")]
    public ReadOnlyMemory<LongId> LongIds { get; }

    /// <summary>
    /// Gets the <c language="csharp">OptionalValues</c> value.
    /// </summary>
    public ReadOnlyMemory<CommandDataReferenceVarIntPair>? OptionalValues { get; }

    /// <summary>
    /// Gets the <c language="csharp">Unknown0</c> value.
    /// </summary>
    public int Unknown0 { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownBoolean0</c> value.
    /// </summary>
    public bool UnknownBoolean0 { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownLongId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownLongId")]
    public LongId? UnknownLongId { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static MapGameChickenTaskStatePayload Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new MapGameChickenTaskStatePayload(
            stream.ReadVarInt(),
            stream.ReadBoolean(),
            MapGameFieldCodec.ReadOptionalLongId(stream),
            MapGameFieldCodec.ReadOptionalDataReferenceVarIntPairs(stream),
            MapGameFieldCodec.ReadLongIds(stream)
        );
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream)
    {
        stream.WriteVarInt(Unknown0);
        stream.WriteBoolean(UnknownBoolean0);
        MapGameFieldCodec.WriteOptionalLongId(stream, UnknownLongId);
        MapGameFieldCodec.WriteOptionalDataReferenceVarIntPairs(stream, OptionalValues);
        MapGameFieldCodec.WriteLongIds(stream, LongIds.Span);
    }
}
