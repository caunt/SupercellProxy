using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration.StickerBook;

/// <summary>Records that the sticker book was opened.</summary>
/// <param name="Seen">The seen.</param>
public sealed record MarkStickerBookSeenMessage(bool Seen) : CodecMessage<MarkStickerBookSeenMessage>, ICodecProvider<MarkStickerBookSeenMessage>
{
    /// <inheritdoc />
    public static ICodec<MarkStickerBookSeenMessage> Codec { get; } = Codecs.Composite(Codecs.Boolean, static message => message.Seen, static (seen) => new MarkStickerBookSeenMessage(seen));
}
