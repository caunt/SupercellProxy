using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration.StickerBook;

/// <summary>Claims the reward of a completed decoration sticker-book collection.</summary>
/// <param name="CollectionIndex">The collection index.</param>
public sealed record ClaimDecoStickerBookCollectionRewardCommand(int CollectionIndex) : CodecCommand<ClaimDecoStickerBookCollectionRewardCommand>, ICommandCodecProvider<ClaimDecoStickerBookCollectionRewardCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimDecoStickerBookCollectionRewardCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.CollectionIndex,
            static (collectionIndex) => new ClaimDecoStickerBookCollectionRewardCommand(collectionIndex)
        ),
        CommandLayout.MetadataFirst
    );
}
