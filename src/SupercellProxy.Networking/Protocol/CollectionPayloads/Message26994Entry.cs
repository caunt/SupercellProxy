using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.CollectionPayloads;

/// <summary>
/// Defines the Message26994 Entry contract.
/// </summary>
/// <summary>
/// Defines the Value0 contract.
/// </summary>
/// <summary>
/// Defines the Value1 contract.
/// </summary>
/// <summary>
/// Defines the Id contract.
/// </summary>
public sealed record Message26994Entry(int Value0, int Value1, [property: System.Text.Json.Serialization.JsonPropertyName("Id")] LongIdentifier Identifier)
{
    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static Message26994Entry Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadVariableInt(), stream.ReadVariableInt(), stream.ReadLongIdentifier());
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVariableInt(Value0);
        stream.WriteVariableInt(Value1);
        stream.WriteLongIdentifier(Identifier);
    }
}
