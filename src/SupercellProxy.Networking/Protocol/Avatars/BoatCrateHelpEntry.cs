using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>
/// Represents <c language="csharp">BoatCrateHelpEntry</c>.
/// </summary>
public sealed record BoatCrateHelpEntry(
    [property: System.Text.Json.Serialization.JsonPropertyName("Unknown0")] int ExpirationTimestamp,
    [property: System.Text.Json.Serialization.JsonPropertyName("Unknown1")] int RequestKind,
    [property: System.Text.Json.Serialization.JsonPropertyName("Unknown2")] int CrateIndex,
    [property: System.Text.Json.Serialization.JsonPropertyName("UnknownId")] LongId? HelperId
)
{
    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static BoatCrateHelpEntry Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadVarInt(), stream.ReadVarInt(), stream.ReadVarInt(), stream.ReadOptionalLongId());
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVarInt(ExpirationTimestamp);
        stream.WriteVarInt(RequestKind);
        stream.WriteVarInt(CrateIndex);
        stream.WriteOptionalLongId(HelperId);
    }
}
