using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;


namespace SupercellProxy.Networking.Protocol.Accounts;

/// <summary>
/// Defines the Clientbound24843 Message contract.
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
    /// Gets the Entries value.
    /// </summary>
    public AccountCandidateEntry[]? Entries { get; init; } = [];

    /// <summary>
    /// Gets the Entry Count value.
    /// </summary>
    public int EntryCount => Entries?.Length ?? -1;

    /// <summary>
    /// Provides the To String value or operation.
    /// </summary>
    public override string ToString()
    {
        return nameof(AccountCandidatesMessage);
    }
}
