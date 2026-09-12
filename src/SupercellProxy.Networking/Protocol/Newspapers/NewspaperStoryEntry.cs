using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Newspapers;

/// <summary>
/// Defines the Newspaper Story Entry contract.
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
/// Defines the Flag contract.
/// </summary>
/// <summary>
/// Defines the Text0 contract.
/// </summary>
/// <summary>
/// Defines the Text1 contract.
/// </summary>
/// <summary>
/// Defines the Text2 contract.
/// </summary>
/// <summary>
/// Defines the Text3 contract.
/// </summary>
/// <summary>
/// Defines the Text4 contract.
/// </summary>
public sealed record NewspaperStoryEntry(int Value0, int Value1, int Value2, bool Flag, string? Text0, string? Text1, string? Text2, string? Text3, string? Text4)
{
    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static NewspaperStoryEntry Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            stream.ReadBoolean(),
            stream.ReadOptionalString(),
            stream.ReadOptionalString(),
            stream.ReadOptionalString(),
            stream.ReadOptionalString(),
            stream.ReadOptionalString()
        );
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVariableInt(Value0);
        stream.WriteVariableInt(Value1);
        stream.WriteVariableInt(Value2);
        stream.WriteBoolean(Flag);
        stream.WriteOptionalString(Text0);
        stream.WriteOptionalString(Text1);
        stream.WriteOptionalString(Text2);
        stream.WriteOptionalString(Text3);
        stream.WriteOptionalString(Text4);
    }
}
