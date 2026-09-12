using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>
/// Represents <c language="csharp">AvatarIdentifierFlagEntry</c>.
/// </summary>
public sealed record AvatarIdentifierFlagEntry(
    [property: System.Text.Json.Serialization.JsonPropertyName("UnknownId")] LongIdentifier? UnknownIdentifier,
    int Unknown0,
    bool Unknown1
)
{
    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static AvatarIdentifierFlagEntry Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadOptionalLongIdentifier(), stream.ReadVariableInt(), stream.ReadBoolean());
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteOptionalLongIdentifier(UnknownIdentifier);
        stream.WriteVariableInt(Unknown0);
        stream.WriteBoolean(Unknown1);
    }
}
