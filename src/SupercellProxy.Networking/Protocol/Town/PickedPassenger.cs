using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>
/// Represents <c language="csharp">PickedPassenger</c>.
/// </summary>
/// <param name="Unknown0">The <c language="csharp">Unknown0</c> value.</param>
/// <param name="Unknown1">The <c language="csharp">Unknown1</c> value.</param>
/// <param name="Unknown2">The <c language="csharp">Unknown2</c> value.</param>
/// <param name="UnknownId0">The <c language="csharp">UnknownId0</c> value.</param>
/// <param name="UnknownId1">The <c language="csharp">UnknownId1</c> value.</param>
/// <param name="UnknownString0">The <c language="csharp">UnknownString0</c> value.</param>
public sealed record PickedPassenger(
    int Unknown0,
    int Unknown1,
    int Unknown2,
    [property: System.Text.Json.Serialization.JsonPropertyName("UnknownId0")] LongId UnknownId0,
    [property: System.Text.Json.Serialization.JsonPropertyName("UnknownId1")] LongId UnknownId1,
    string? UnknownString0
)
{
    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static PickedPassenger Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadLongId(),
            stream.ReadLongId(),
            stream.ReadOptionalString()
        );
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteVarInt(Unknown2);
        stream.WriteLongId(UnknownId0);
        stream.WriteLongId(UnknownId1);
        stream.WriteOptionalString(UnknownString0);
    }
}
