using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>
/// <para>Native optional logic-long and three-value map-game configuration entry.</para>
/// </summary>
public sealed record MapGameConfigurationEntry(
    [property: System.Text.Json.Serialization.JsonPropertyName("UnknownLongId")] LongIdentifier? UnknownLongIdentifier,
    int Unknown0,
    int Unknown1,
    int Unknown2
)
{
    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static MapGameConfigurationEntry Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new MapGameConfigurationEntry(
            MapGameFieldCodec.ReadOptionalLongIdentifier(stream),
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt()
        );
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        MapGameFieldCodec.WriteOptionalLongIdentifier(stream, UnknownLongIdentifier);
        stream.WriteVariableInt(Unknown0);
        stream.WriteVariableInt(Unknown1);
        stream.WriteVariableInt(Unknown2);
    }
}
