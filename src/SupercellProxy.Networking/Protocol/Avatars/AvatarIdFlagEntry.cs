using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>
/// Represents <c language="csharp">AvatarIdFlagEntry</c>.
/// </summary>
public sealed record AvatarIdFlagEntry([property: System.Text.Json.Serialization.JsonPropertyName("UnknownId")] LongId? UnknownId, int Unknown0, bool Unknown1)
{
    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static AvatarIdFlagEntry Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadOptionalLongId(), stream.ReadVarInt(), stream.ReadBoolean());
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteOptionalLongId(UnknownId);
        stream.WriteVarInt(Unknown0);
        stream.WriteBoolean(Unknown1);
    }
}
