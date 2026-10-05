using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MapGame.Tasks;

/// <summary>
/// Represents <c language="csharp">MapGameSanctuaryAnimalTaskStatePayload</c>.
/// </summary>
/// <param name="Unknown0">The <c language="csharp">Unknown0</c> value.</param>
/// <param name="Unknown1">The <c language="csharp">Unknown1</c> value.</param>
/// <param name="AnimalGlobalId">The sanctuary-animal definition.</param>
/// <param name="Collected">Whether a pawn has collected the animal.</param>
/// <param name="UnknownBoolean1">The <c language="csharp">UnknownBoolean1</c> value.</param>
/// <param name="Escaped">Whether the collected animal has escaped.</param>
/// <param name="EscapeNodeId">The node selected for the escape.</param>
/// <param name="EscapeCount">The retained wrapping escape counter.</param>
/// <param name="CollectorId">The collecting pawn's avatar id.</param>
public sealed record MapGameSanctuaryAnimalTaskStatePayload(
    int Unknown0,
    int Unknown1,
    [property: System.Text.Json.Serialization.JsonPropertyName("UnknownGlobalId0")] int AnimalGlobalId,
    [property: System.Text.Json.Serialization.JsonPropertyName("UnknownBoolean0")] bool Collected,
    bool UnknownBoolean1,
    [property: System.Text.Json.Serialization.JsonPropertyName("UnknownBoolean2")] bool Escaped,
    [property: System.Text.Json.Serialization.JsonPropertyName("Unknown2")] int EscapeNodeId,
    [property: System.Text.Json.Serialization.JsonPropertyName("Unknown3")] int EscapeCount,
    [property: System.Text.Json.Serialization.JsonPropertyName("UnknownPair0")] LongId? CollectorId
) : MapGameTaskStatePayload
{
    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static MapGameSanctuaryAnimalTaskStatePayload Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new MapGameSanctuaryAnimalTaskStatePayload(
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadBoolean(),
            stream.ReadBoolean(),
            stream.ReadBoolean(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            MapGameFieldCodec.ReadOptionalLongId(stream)
        );
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream)
    {
        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteVarInt(AnimalGlobalId);
        stream.WriteBoolean(Collected);
        stream.WriteBoolean(UnknownBoolean1);
        stream.WriteBoolean(Escaped);
        stream.WriteVarInt(EscapeNodeId);
        stream.WriteVarInt(EscapeCount);
        MapGameFieldCodec.WriteOptionalLongId(stream, CollectorId);
    }
}
