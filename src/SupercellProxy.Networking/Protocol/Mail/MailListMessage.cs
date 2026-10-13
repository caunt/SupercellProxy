using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Mail;

/// Carries the player's mail entries in native wire order.
public sealed record MailListMessage : CodecMessage<MailListMessage>, ICodecProvider<MailListMessage>
{
    private const int MaximumEntryCount = 1024;

    /// <inheritdoc />
    public static ICodec<MailListMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            Codecs.NullableArray(MailEntry.Codec, CollectionCount.PerElement(MailEntry.MinimumEncodedSize).WithMaximum(MaximumEntryCount).WithStrictAbsence()),
            static message => message.Entries,
            static (entries) => new MailListMessage
            {
                Entries = entries,
            }
        ),
        trailingDataMessage: "The mail-list message has trailing data."
    );

    /// Gets the decoded mail entries, or null for the native absent-list marker.
    public MailEntry[]? Entries { get; init; }

    /// Gets the decoded entry count.
    public int EntryCount => Entries?.Length ?? -1;

    /// Omits private mail content from diagnostic text.
    public override string ToString()
    {
        return nameof(MailListMessage);
    }
}
