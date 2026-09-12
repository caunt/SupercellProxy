using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>
/// Represents <c language="csharp">PickedPassenger</c>.
/// </summary>
/// <param name="Unknown0">The <c language="csharp">Unknown0</c> value.</param>
/// <param name="Unknown1">The <c language="csharp">Unknown1</c> value.</param>
/// <param name="Unknown2">The <c language="csharp">Unknown2</c> value.</param>
/// <param name="UnknownIdentifier0">The <c language="csharp">UnknownId0</c> value.</param>
/// <param name="UnknownIdentifier1">The <c language="csharp">UnknownId1</c> value.</param>
/// <param name="UnknownString0">The <c language="csharp">UnknownString0</c> value.</param>
public sealed record PickedPassenger(
    int Unknown0,
    int Unknown1,
    int Unknown2,
    [property: System.Text.Json.Serialization.JsonPropertyName("UnknownId0")] LongIdentifier UnknownIdentifier0,
    [property: System.Text.Json.Serialization.JsonPropertyName("UnknownId1")] LongIdentifier UnknownIdentifier1,
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
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            stream.ReadLongIdentifier(),
            stream.ReadLongIdentifier(),
            stream.ReadOptionalString()
        );
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
        stream.WriteLongIdentifier(UnknownIdentifier0);
        stream.WriteLongIdentifier(UnknownIdentifier1);
        stream.WriteOptionalString(UnknownString0);
    }
}
