using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>
/// Represents <c language="csharp">AvatarIdPairEntry</c>.
/// </summary>
public sealed record AvatarIdPairEntry(
    [property: System.Text.Json.Serialization.JsonPropertyName("UnknownId")] LongId? UnknownId,
    int Unknown0,
    int Unknown1,
    bool Unknown2
)
{
    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static AvatarIdPairEntry Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadOptionalLongId(), stream.ReadVarInt(), stream.ReadVarInt(), stream.ReadBoolean());
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteOptionalLongId(UnknownId);
        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteBoolean(Unknown2);
    }
}
