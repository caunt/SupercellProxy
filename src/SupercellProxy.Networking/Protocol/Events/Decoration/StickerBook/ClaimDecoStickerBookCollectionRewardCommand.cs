using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration.StickerBook;

/// <summary>Acknowledges a sticker-book collection: its newly collected decorations are marked seen and its notifications clear.</summary>
/// <param name="CollectionIndex">Position of the collection in the sticker-book collection table.</param>
public sealed record ClaimDecoStickerBookCollectionRewardCommand(int CollectionIndex) : CodecCommand<ClaimDecoStickerBookCollectionRewardCommand>, ICommandCodecProvider<ClaimDecoStickerBookCollectionRewardCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimDecoStickerBookCollectionRewardCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.CollectionIndex,
            static collectionIndex => new ClaimDecoStickerBookCollectionRewardCommand(collectionIndex)
        ),
        CommandLayout.MetadataFirst
    );
}
