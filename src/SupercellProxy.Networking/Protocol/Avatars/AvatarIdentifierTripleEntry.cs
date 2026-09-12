using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>
/// Represents <c language="csharp">AvatarIdentifierTripleEntry</c>.
/// </summary>
public sealed record AvatarIdentifierTripleEntry(
    int Unknown0,
    int Unknown1,
    int Unknown2,
    [property: System.Text.Json.Serialization.JsonPropertyName("UnknownId")] LongIdentifier? UnknownIdentifier
)
{
    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static AvatarIdentifierTripleEntry Decode(MessageStream stream)
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
        stream.WriteVariableInt(Unknown0);
        stream.WriteVariableInt(Unknown1);
        stream.WriteVariableInt(Unknown2);
        stream.WriteOptionalLongIdentifier(UnknownIdentifier);
    }
}
