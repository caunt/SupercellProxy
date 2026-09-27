using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>
/// Represents <c language="csharp">BoatCrateHelpEntry</c>.
/// </summary>
public sealed record BoatCrateHelpEntry(
    [property: System.Text.Json.Serialization.JsonPropertyName("Unknown0")] int ExpirationTimestamp,
    [property: System.Text.Json.Serialization.JsonPropertyName("Unknown1")] int RequestKind,
    [property: System.Text.Json.Serialization.JsonPropertyName("Unknown2")] int CrateIndex,
    [property: System.Text.Json.Serialization.JsonPropertyName("UnknownId")] LongIdentifier? HelperIdentifier
)
{
    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static BoatCrateHelpEntry Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadVariableInt(), stream.ReadVariableInt(), stream.ReadVariableInt(), stream.ReadOptionalLongIdentifier());
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVariableInt(ExpirationTimestamp);
        stream.WriteVariableInt(RequestKind);
        stream.WriteVariableInt(CrateIndex);
        stream.WriteOptionalLongIdentifier(HelperIdentifier);
    }
}
