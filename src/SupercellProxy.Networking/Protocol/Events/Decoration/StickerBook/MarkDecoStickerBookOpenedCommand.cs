using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration.StickerBook;

/// <summary>Records that the decoration sticker book was opened once its feature is unlocked.</summary>
public sealed record MarkDecoStickerBookOpenedCommand : CodecCommand<MarkDecoStickerBookOpenedCommand>, ICommandCodecProvider<MarkDecoStickerBookOpenedCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkDecoStickerBookOpenedCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new MarkDecoStickerBookOpenedCommand()));
}
