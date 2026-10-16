using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration.StickerBook;

/// <summary>Claims the reward of a decoration sticker-book page.</summary>
/// <param name="PageIndex">Position of the filled page in the sticker book, starting at 0.</param>
public sealed record ClaimDecoStickerBookPageRewardCommand(int PageIndex) : CodecCommand<ClaimDecoStickerBookPageRewardCommand>, ICommandCodecProvider<ClaimDecoStickerBookPageRewardCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimDecoStickerBookPageRewardCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.PageIndex, static pageIndex => new ClaimDecoStickerBookPageRewardCommand(pageIndex)),
        CommandLayout.MetadataFirst
    );
}
