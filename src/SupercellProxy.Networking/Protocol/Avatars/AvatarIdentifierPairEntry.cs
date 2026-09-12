using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>
/// Represents <c language="csharp">AvatarIdentifierPairEntry</c>.
/// </summary>
public sealed record AvatarIdentifierPairEntry(
    [property: System.Text.Json.Serialization.JsonPropertyName("UnknownId")] LongIdentifier? UnknownIdentifier,
    int Unknown0,
    int Unknown1,
    bool Unknown2
)
{
    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static AvatarIdentifierPairEntry Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadOptionalLongIdentifier(), stream.ReadVariableInt(), stream.ReadVariableInt(), stream.ReadBoolean());
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteOptionalLongIdentifier(UnknownIdentifier);
        stream.WriteVariableInt(Unknown0);
        stream.WriteVariableInt(Unknown1);
        stream.WriteBoolean(Unknown2);
    }
}
