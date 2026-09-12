using SupercellProxy.Networking.Protocol.CommandEncoding.CollectionFields;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MapGame.Tasks;

/// <summary>
/// Represents <c language="csharp">MapGameObstacleTaskStatePayload</c>.
/// </summary>
public sealed record MapGameObstacleTaskStatePayload : MapGameTaskStatePayload
{
    /// <summary>
    /// Initializes a new <see cref="MapGameObstacleTaskStatePayload"/> instance.
    /// </summary>
    public MapGameObstacleTaskStatePayload(
        bool unknownBoolean0,
        bool unknownBoolean1,
        LongIdentifier? unknownLongIdentifier,
        ReadOnlyMemory<CommandDataReferenceVariableIntPair>? optionalValues
    )
    {
        UnknownBoolean0 = unknownBoolean0;
        UnknownBoolean1 = unknownBoolean1;
        UnknownLongIdentifier = unknownLongIdentifier;
        OptionalValues = optionalValues?.ToArray();
    }

    /// <summary>
    /// Gets the <c language="csharp">OptionalValues</c> value.
    /// </summary>
    public ReadOnlyMemory<CommandDataReferenceVariableIntPair>? OptionalValues { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownBoolean0</c> value.
    /// </summary>
    public bool UnknownBoolean0 { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownBoolean1</c> value.
    /// </summary>
    public bool UnknownBoolean1 { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownLongId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownLongId")]
    public LongIdentifier? UnknownLongIdentifier { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static MapGameObstacleTaskStatePayload Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new MapGameObstacleTaskStatePayload(
            stream.ReadBoolean(),
            stream.ReadBoolean(),
            MapGameFieldCodec.ReadOptionalLongIdentifier(stream),
            MapGameFieldCodec.ReadOptionalDataReferenceVariableIntPairs(stream)
        );
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream)
    {
        stream.WriteBoolean(UnknownBoolean0);
        stream.WriteBoolean(UnknownBoolean1);
        MapGameFieldCodec.WriteOptionalLongIdentifier(stream, UnknownLongIdentifier);
        MapGameFieldCodec.WriteOptionalDataReferenceVariableIntPairs(stream, OptionalValues);
    }
}
