using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts;

/// <summary>
/// One farm in the list <see cref="AccountCandidatesMessage"/> carries. The layout is native's friend entry (see
/// <see cref="Friends.Entries.FriendEntry"/>), so the fields mean the same as there.
/// </summary>
/// <param name="AccountId">Home id of the listed farm.</param>
/// <param name="Text0">Farm name; null when the farm has none.</param>
/// <param name="Text1">Facebook id linked to the farm, if any.</param>
/// <param name="Text2">Game Center id linked to the farm, if any.</param>
/// <param name="Text3">Address of the farm's profile picture, if any.</param>
/// <param name="Value0">The farm's experience level.</param>
/// <param name="Value1">The friend entry's sort value.</param>
/// <param name="Value2">Relationship status between the player and this farm (friend, incoming or outgoing request).</param>
/// <param name="Value3">The friend entry's block time stamp.</param>
public sealed record AccountCandidateEntry(
    [property: JsonPropertyName("AccountId")] LongId AccountId,
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
    /// Returns only the type name, keeping farm names and linked social ids out of logs.
    /// </summary>
    public override string ToString()
    {
        return nameof(AccountCandidateEntry);
    }
}
