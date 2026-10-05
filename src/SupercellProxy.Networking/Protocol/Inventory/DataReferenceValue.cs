using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Inventory;

/// <summary>
/// Represents <c language="csharp">DataReferenceValue</c>.
/// </summary>
public sealed record DataReferenceValue([property: System.Text.Json.Serialization.JsonPropertyName("GlobalDataId")] int GlobalDataId, int Value)
{
    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static DataReferenceValue Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadVarInt(), stream.ReadVarInt());
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVarInt(GlobalDataId);
        stream.WriteVarInt(Value);
    }
}
