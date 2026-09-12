using SupercellProxy.Networking.Protocol.CommandEncoding.CollectionFields;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MapGame.Tasks;

/// <summary>
/// Represents <c language="csharp">MapGameGasStationTaskStatePayload</c>.
/// </summary>
public sealed record MapGameGasStationTaskStatePayload : MapGameTaskStatePayload
{
    /// <summary>
    /// Initializes a new <see cref="MapGameGasStationTaskStatePayload"/> instance.
    /// </summary>
    public MapGameGasStationTaskStatePayload(bool unknownBoolean0, bool unknownBoolean1, int unknown0, ReadOnlyMemory<CommandDataReferenceVariableIntPair>? optionalValues)
    {
        UnknownBoolean0 = unknownBoolean0;
        UnknownBoolean1 = unknownBoolean1;
        Unknown0 = unknown0;
        OptionalValues = optionalValues is null
            ? null
            : (ReadOnlyMemory<CommandDataReferenceVariableIntPair>?)optionalValues.Value.ToArray();
    }

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
    /// Gets the <c language="csharp">UnknownBoolean1</c> value.
    /// </summary>
    public bool UnknownBoolean1 { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static MapGameGasStationTaskStatePayload Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new MapGameGasStationTaskStatePayload(
            stream.ReadBoolean(),
            stream.ReadBoolean(),
            stream.ReadVariableInt(),
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
        stream.WriteVariableInt(Unknown0);
        MapGameFieldCodec.WriteOptionalDataReferenceVariableIntPairs(stream, OptionalValues);
    }
}
