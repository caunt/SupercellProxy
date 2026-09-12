using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MapGame.Tasks;

/// <summary>
/// Represents <c language="csharp">MapGameSanctuaryAnimalTaskStatePayload</c>.
/// </summary>
/// <param name="Unknown0">The <c language="csharp">Unknown0</c> value.</param>
/// <param name="Unknown1">The <c language="csharp">Unknown1</c> value.</param>
/// <param name="UnknownGlobalIdentifier0">The <c language="csharp">UnknownGlobalId0</c> value.</param>
/// <param name="UnknownBoolean0">The <c language="csharp">UnknownBoolean0</c> value.</param>
/// <param name="UnknownBoolean1">The <c language="csharp">UnknownBoolean1</c> value.</param>
/// <param name="UnknownBoolean2">The <c language="csharp">UnknownBoolean2</c> value.</param>
/// <param name="Unknown2">The <c language="csharp">Unknown2</c> value.</param>
/// <param name="Unknown3">The <c language="csharp">Unknown3</c> value.</param>
/// <param name="UnknownPair0">The <c language="csharp">UnknownPair0</c> value.</param>
public sealed record MapGameSanctuaryAnimalTaskStatePayload(
    int Unknown0,
    int Unknown1,
    [property: System.Text.Json.Serialization.JsonPropertyName("UnknownGlobalId0")] int UnknownGlobalIdentifier0,
    bool UnknownBoolean0,
    bool UnknownBoolean1,
    bool UnknownBoolean2,
    int Unknown2,
    int Unknown3,
    LongIdentifier? UnknownPair0
) : MapGameTaskStatePayload
{
    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static MapGameSanctuaryAnimalTaskStatePayload Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new MapGameSanctuaryAnimalTaskStatePayload(
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            stream.ReadBoolean(),
            stream.ReadBoolean(),
            stream.ReadBoolean(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            MapGameFieldCodec.ReadOptionalLongIdentifier(stream)
        );
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream)
    {
        stream.WriteVariableInt(Unknown0);
        stream.WriteVariableInt(Unknown1);
        stream.WriteVariableInt(UnknownGlobalIdentifier0);
        stream.WriteBoolean(UnknownBoolean0);
        stream.WriteBoolean(UnknownBoolean1);
        stream.WriteBoolean(UnknownBoolean2);
        stream.WriteVariableInt(Unknown2);
        stream.WriteVariableInt(Unknown3);
        MapGameFieldCodec.WriteOptionalLongIdentifier(stream, UnknownPair0);
    }
}
