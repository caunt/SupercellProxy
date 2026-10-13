using SupercellProxy.Networking.Protocol.Serialization;

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
) : ICodecProvider<AccountCandidateEntry>
{
    /// <inheritdoc />
    public static ICodec<AccountCandidateEntry> Codec { get; } = Codecs.Composite(
        Codecs.LongId,
        static value => value.AccountId,
        Codecs.OptionalString,
        static value => value.Text0,
        Codecs.OptionalString,
        static value => value.Text1,
        Codecs.OptionalString,
        static value => value.Text2,
        Codecs.OptionalString,
        static value => value.Text3,
        Codecs.VarInt,
        static value => value.Value0,
        Codecs.VarInt,
        static value => value.Value1,
        Codecs.VarInt,
        static value => value.Value2,
        Codecs.VarInt,
        static value => value.Value3,
        static (accountId, text0, text1, text2, text3, value0, value1, value2, value3) => new AccountCandidateEntry(accountId, text0, text1, text2, text3, value0, value1, value2, value3)
    );

    /// <summary>
    /// Provides the To String value or operation.
    /// </summary>
    public override string ToString()
    {
        return nameof(AccountCandidateEntry);
    }
}
