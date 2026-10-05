using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>
/// <para>Native optional logic-long and three-value map-game configuration entry.</para>
/// </summary>
public sealed record MapGameConfigurationEntry(
    [property: System.Text.Json.Serialization.JsonPropertyName("UnknownLongId")] LongId? UnknownLongId,
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

        return new MapGameConfigurationEntry(MapGameFieldCodec.ReadOptionalLongId(stream), stream.ReadVarInt(), stream.ReadVarInt(), stream.ReadVarInt());
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        MapGameFieldCodec.WriteOptionalLongId(stream, UnknownLongId);
        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteVarInt(Unknown2);
    }
}
