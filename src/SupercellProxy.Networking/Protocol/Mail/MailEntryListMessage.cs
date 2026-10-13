using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Mail;

/// <summary>Lists mail entries; the client ignores it.</summary>
/// <param name="Entries">The entries.</param>
/// <param name="Flag">The flag.</param>
public sealed record MailEntryListMessage(ReadOnlyMemory<SupercellProxy.Networking.Protocol.Mail.MailEntry>? Entries, bool Flag) : CodecMessage<MailEntryListMessage>, ICodecProvider<MailEntryListMessage>
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
