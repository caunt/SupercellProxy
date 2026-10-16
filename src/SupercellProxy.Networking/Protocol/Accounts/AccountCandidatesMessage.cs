using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;


namespace SupercellProxy.Networking.Protocol.Accounts;

/// <summary>
/// Clientbound list of farms in native's friend entry layout. Native hands these entries to the player's following list, so this is
/// most likely native's FollowingListMessage (exploration naming audit, 2026-10-06), not an account selection list.
/// </summary>
public sealed record AccountCandidatesMessage : CodecMessage<AccountCandidatesMessage>, ICodecProvider<AccountCandidatesMessage>
{
    /// <inheritdoc />
    public static ICodec<AccountCandidatesMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            Codecs.NullableArray(AccountCandidateEntry.Codec, CollectionCount.PerElement(minimumElementSize: 28).WithStrictAbsence()),
            static message => message.Entries,
            static entries => new AccountCandidatesMessage { Entries = entries }
        ),
        trailingDataMessage: "The account-entry response has trailing data."
    );

    /// <summary>
    /// The listed farms; null when the server sent the list as absent rather than empty.
    /// </summary>
    public AccountCandidateEntry[]? Entries { get; init; } = [];

    /// <summary>
    /// Number of listed farms, or -1 when the list was sent as absent.
    /// </summary>
    public int EntryCount => Entries?.Length ?? -1;

    /// <summary>
    /// Returns only the type name, keeping the listed farms out of logs.
    /// </summary>
    public override string ToString()
    {
        return nameof(AccountCandidatesMessage);
    }
}
