using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Inventory;

/// <summary>
/// Represents <c language="csharp">DataReferenceValue</c>.
/// </summary>
public sealed record DataReferenceValue([property: System.Text.Json.Serialization.JsonPropertyName("GlobalDataId")] int GlobalDataIdentifier, int Value)
{
    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static DataReferenceValue Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadVariableInt(), stream.ReadVariableInt());
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVariableInt(GlobalDataIdentifier);
        stream.WriteVariableInt(Value);
    }
}
