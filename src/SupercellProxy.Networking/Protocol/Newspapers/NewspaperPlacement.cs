using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Newspapers;

/// <summary>
/// Defines the Newspaper Placement contract.
/// </summary>
/// <summary>
/// Defines the Index contract.
/// </summary>
/// <summary>
/// Defines the Home Id contract.
/// </summary>
/// <summary>
/// Defines the Value0 contract.
/// </summary>
/// <summary>
/// Defines the Value1 contract.
/// </summary>
/// <summary>
/// Defines the Value2 contract.
/// </summary>
/// <summary>
/// Defines the Value3 contract.
/// </summary>
/// <summary>
/// Defines the Flag contract.
/// </summary>
public sealed record NewspaperPlacement(
    int Index,
    [property: System.Text.Json.Serialization.JsonPropertyName("HomeId")] LongIdentifier HomeIdentifier,
    int Value0,
    int Value1,
    int Value2,
    int Value3,
    bool Flag
)
{
    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static NewspaperPlacement Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(
            stream.ReadVariableInt(),
            stream.ReadLongIdentifier(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            stream.ReadBoolean()
        );
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVariableInt(Index);
        stream.WriteLongIdentifier(HomeIdentifier);
        stream.WriteVariableInt(Value0);
        stream.WriteVariableInt(Value1);
        stream.WriteVariableInt(Value2);
        stream.WriteVariableInt(Value3);
        stream.WriteBoolean(Flag);
    }
}
