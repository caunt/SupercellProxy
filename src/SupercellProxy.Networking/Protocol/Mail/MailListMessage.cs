using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Mail;

/// <summary>Carries the player's mail entries in native wire order.</summary>
public sealed record MailListMessage : CodecMessage<MailListMessage>, ICodecProvider<MailListMessage>
{
    private const int MaximumEntryCount = 1024;

    /// <inheritdoc />
    public static ICodec<MailListMessage> Codec { get; } = Codecs.Complete(Codecs.Composite(Codecs.NullableArray(MailEntry.Codec, CollectionCount.PerElement(MailEntry.MinimumEncodedSize).With(MaximumEntryCount).WithStrictAbsence()),
            static message => message.Entries,
            static entries => new MailListMessage
            {
                Entries = entries,
            }),
        trailingDataMessage: "The mail-list message has trailing data.");

    /// <summary>Gets or sets the decoded mail entries, or <see langword="null"/> for the native absent-list marker.</summary>
    /// <value>The whole mailbox in wire order, at most 1024 letters.</value>
    public MailEntry[]? Entries { get; init; }

    /// <summary>Gets the decoded entry count.</summary>
    /// <value>The number of letters in <see cref="Entries"/>, or -1 when the message carries the absent-list marker.</value>
    public int EntryCount => Entries?.Length ?? -1;

    /// <summary>Omits private mail content from diagnostic text.</summary>
    /// <returns>Always the type name <c>MailListMessage</c>, so letters, names and linked account ids never reach logs.</returns>
    public override string ToString()
    {
        return nameof(MailListMessage);
    }
}
