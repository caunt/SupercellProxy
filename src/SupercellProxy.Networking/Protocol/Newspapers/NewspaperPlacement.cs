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
    [property: System.Text.Json.Serialization.JsonPropertyName("HomeId")] LongId HomeId,
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
            stream.ReadVarInt(),
            stream.ReadLongId(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadBoolean()
        );
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVarInt(Index);
        stream.WriteLongId(HomeId);
        stream.WriteVarInt(Value0);
        stream.WriteVarInt(Value1);
        stream.WriteVarInt(Value2);
        stream.WriteVarInt(Value3);
        stream.WriteBoolean(Flag);
    }
}
