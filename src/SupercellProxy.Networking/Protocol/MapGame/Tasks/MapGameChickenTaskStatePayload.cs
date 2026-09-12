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
        LongIdentifier? unknownLongIdentifier,
        ReadOnlyMemory<CommandDataReferenceVariableIntPair>? optionalValues,
        ReadOnlyMemory<LongIdentifier> logicLongs
    )
    {
        Unknown0 = unknown0;
        UnknownBoolean0 = unknownBoolean0;
        UnknownLongIdentifier = unknownLongIdentifier;
        OptionalValues = optionalValues is null
            ? null
            : (ReadOnlyMemory<CommandDataReferenceVariableIntPair>?)optionalValues.Value.ToArray();
        LongIdentifiers = logicLongs.ToArray();
    }

    /// <summary>
    /// Gets the <c language="csharp">LongIds</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("LongIds")]
    public ReadOnlyMemory<LongIdentifier> LongIdentifiers { get; }

    /// <summary>
    /// Gets the <c language="csharp">OptionalValues</c> value.
    /// </summary>
    public ReadOnlyMemory<CommandDataReferenceVariableIntPair>? OptionalValues { get; }

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
    public LongIdentifier? UnknownLongIdentifier { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static MapGameChickenTaskStatePayload Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new MapGameChickenTaskStatePayload(
            stream.ReadVariableInt(),
            stream.ReadBoolean(),
            MapGameFieldCodec.ReadOptionalLongIdentifier(stream),
            MapGameFieldCodec.ReadOptionalDataReferenceVariableIntPairs(stream),
            MapGameFieldCodec.ReadLongIdentifiers(stream)
        );
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream)
    {
        stream.WriteVariableInt(Unknown0);
        stream.WriteBoolean(UnknownBoolean0);
        MapGameFieldCodec.WriteOptionalLongIdentifier(stream, UnknownLongIdentifier);
        MapGameFieldCodec.WriteOptionalDataReferenceVariableIntPairs(stream, OptionalValues);
        MapGameFieldCodec.WriteLongIdentifiers(stream, LongIdentifiers.Span);
    }
}
