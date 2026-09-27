using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MapGame.Tasks;

/// <summary>
/// Represents <c language="csharp">MapGameSanctuaryAnimalTaskStatePayload</c>.
/// </summary>
/// <param name="Unknown0">The <c language="csharp">Unknown0</c> value.</param>
/// <param name="Unknown1">The <c language="csharp">Unknown1</c> value.</param>
/// <param name="AnimalGlobalIdentifier">The sanctuary-animal definition.</param>
/// <param name="Collected">Whether a pawn has collected the animal.</param>
/// <param name="UnknownBoolean1">The <c language="csharp">UnknownBoolean1</c> value.</param>
/// <param name="Escaped">Whether the collected animal has escaped.</param>
/// <param name="EscapeNodeIdentifier">The node selected for the escape.</param>
/// <param name="EscapeCount">The retained wrapping escape counter.</param>
/// <param name="CollectorIdentifier">The collecting pawn's avatar identifier.</param>
public sealed record MapGameSanctuaryAnimalTaskStatePayload(
    int Unknown0,
    int Unknown1,
    [property: System.Text.Json.Serialization.JsonPropertyName("UnknownGlobalId0")] int AnimalGlobalIdentifier,
    [property: System.Text.Json.Serialization.JsonPropertyName("UnknownBoolean0")] bool Collected,
    bool UnknownBoolean1,
    [property: System.Text.Json.Serialization.JsonPropertyName("UnknownBoolean2")] bool Escaped,
    [property: System.Text.Json.Serialization.JsonPropertyName("Unknown2")] int EscapeNodeIdentifier,
    [property: System.Text.Json.Serialization.JsonPropertyName("Unknown3")] int EscapeCount,
    [property: System.Text.Json.Serialization.JsonPropertyName("UnknownPair0")] LongIdentifier? CollectorIdentifier
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
        stream.WriteVariableInt(AnimalGlobalIdentifier);
        stream.WriteBoolean(Collected);
        stream.WriteBoolean(UnknownBoolean1);
        stream.WriteBoolean(Escaped);
        stream.WriteVariableInt(EscapeNodeIdentifier);
        stream.WriteVariableInt(EscapeCount);
        MapGameFieldCodec.WriteOptionalLongIdentifier(stream, CollectorIdentifier);
    }
}
