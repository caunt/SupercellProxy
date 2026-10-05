using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Accounts;

/// <summary>
/// Defines the Account Candidate Entry contract.
/// </summary>
/// <summary>
/// Defines the Account Id contract.
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
public sealed record AccountCandidateEntry(
    [property: System.Text.Json.Serialization.JsonPropertyName("AccountId")] LongId AccountId,
    string? Text0,
    string? Text1,
    string? Text2,
    string? Text3,
    int Value0,
    int Value1,
    int Value2,
    int Value3
)
{
    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static AccountCandidateEntry Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(
            stream.ReadLongId(),
            stream.ReadOptionalString(),
            stream.ReadOptionalString(),
            stream.ReadOptionalString(),
            stream.ReadOptionalString(),
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
        stream.WriteLongId(AccountId);
        stream.WriteOptionalString(Text0);
        stream.WriteOptionalString(Text1);
        stream.WriteOptionalString(Text2);
        stream.WriteOptionalString(Text3);
        stream.WriteVarInt(Value0);
        stream.WriteVarInt(Value1);
        stream.WriteVarInt(Value2);
        stream.WriteVarInt(Value3);
    }

    /// <summary>
    /// Provides the To String value or operation.
    /// </summary>
    public override string ToString()
    {
        return nameof(AccountCandidateEntry);
    }
}
