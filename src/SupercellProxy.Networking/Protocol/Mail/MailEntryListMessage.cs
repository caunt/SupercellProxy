using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Mail;

/// <summary>Lists mail entries; the client ignores it.</summary>
/// <param name="Entries">The letters listed; null when the list is absent.</param>
/// <param name="Flag">Switch sent after the list; its meaning is not identified.</param>
public sealed record MailEntryListMessage(ReadOnlyMemory<MailEntry>? Entries, bool Flag) : CodecMessage<MailEntryListMessage>, ICodecProvider<MailEntryListMessage>
{
    /// <inheritdoc />
    public static ICodec<MailEntryListMessage> Codec { get; } = Codecs.Composite(
        Codecs.NullableMemory(MailEntry.Codec),
        static message => message.Entries,
        Codecs.Boolean,
        static message => message.Flag,
        static (entries, flag) => new MailEntryListMessage(entries, flag)
    );
}
