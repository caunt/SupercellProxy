using System.Collections.Immutable;

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
    public static ICodec<AccountCandidatesMessage> Codec { get; } = Codecs.Complete(Codecs.Composite(Codecs.NullableArray(AccountCandidateEntry.Codec, CollectionCount.PerElement(minimumElementSize: 28).WithStrictAbsence()),
            static message => message.Entries,
            static entries => new AccountCandidatesMessage { Entries = entries }),
        trailingDataMessage: "The account-entry response has trailing data.");

    /// <summary>
    /// Gets or sets the listed farms; <see langword="null"/> when the server sent the list as absent rather than empty.
    /// </summary>
    /// <value>The entries in wire order, each at least 28 bytes on the wire; an empty array by default. A <see langword="null"/> value is written back as an absent list.</value>
    public ImmutableArray<AccountCandidateEntry>? Entries { get; init; } = [];

    /// <summary>
    /// Gets the number of listed farms, or -1 when the list was sent as absent.
    /// </summary>
    /// <value>The length of <see cref="Entries"/>, 0 for an empty list, or -1 when <see cref="Entries"/> is <see langword="null"/>.</value>
    public int EntryCount => Entries?.Length ?? -1;

    /// <summary>
    /// Returns only the type name, keeping the listed farms out of logs.
    /// </summary>
    /// <returns>Always the text <c>AccountCandidatesMessage</c>, however many farms are listed.</returns>
    public override string ToString()
    {
        return nameof(AccountCandidatesMessage);
    }
}
