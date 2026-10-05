using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>
/// Represents <c language="csharp">NeighborhoodData</c>.
/// </summary>
/// <param name="NeighborhoodId">The <c language="csharp">NeighborhoodId</c> value.</param>
/// <param name="NeighborhoodName">The <c language="csharp">NeighborhoodName</c> value.</param>
/// <param name="NeighborhoodRole">The <c language="csharp">NeighborhoodRole</c> value.</param>
/// <param name="BadgeUnknown0">The <c language="csharp">BadgeUnknown0</c> value.</param>
/// <param name="BadgeUnknown1">The <c language="csharp">BadgeUnknown1</c> value.</param>
/// <param name="BadgeUnknown2">The <c language="csharp">BadgeUnknown2</c> value.</param>
/// <param name="Unknown0">The <c language="csharp">Unknown0</c> value.</param>
/// <param name="Unknown1">The <c language="csharp">Unknown1</c> value.</param>
/// <param name="Unknown2">The <c language="csharp">Unknown2</c> value.</param>
public sealed record NeighborhoodData(
    [property: System.Text.Json.Serialization.JsonPropertyName("NeighborhoodId")] LongId NeighborhoodId,
    string? NeighborhoodName,
    int NeighborhoodRole,
    int BadgeUnknown0,
    int BadgeUnknown1,
    int BadgeUnknown2,
    int Unknown0,
    int Unknown1,
    int Unknown2
)
{
    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static NeighborhoodData Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(
            stream.ReadLongId(),
            stream.ReadOptionalString(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt()
        );
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteLongId(NeighborhoodId);
        stream.WriteOptionalString(NeighborhoodName);
        stream.WriteVarInt(NeighborhoodRole);
        stream.WriteVarInt(BadgeUnknown0);
        stream.WriteVarInt(BadgeUnknown1);
        stream.WriteVarInt(BadgeUnknown2);
        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteVarInt(Unknown2);
    }
}
