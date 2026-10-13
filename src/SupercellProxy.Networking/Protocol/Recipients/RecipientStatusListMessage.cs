using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Recipients;

/// <summary>Delivers recipient statuses to the social manager.</summary>
/// <param name="Entries">The entries.</param>
public sealed record RecipientStatusListMessage(ReadOnlyMemory<RecipientStatus> Entries) : CodecMessage<RecipientStatusListMessage>, ICodecProvider<RecipientStatusListMessage>
{
    /// <inheritdoc />
    public static ICodec<RecipientStatusListMessage> Codec { get; } = Codecs.Composite(
        Codecs.Memory(RecipientStatus.Codec),
        static message => message.Entries,
        static (entries) => new RecipientStatusListMessage(entries)
    );
}
