using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration.StickerBook;

/// <summary>Claims a decoration sticker-book collection reward.</summary>
/// <param name="RewardIndex">Which of the collection's rewards to claim.</param>
/// <param name="CollectionIndex">Position of the collection in the sticker book.</param>
public sealed record ClaimDecoStickerBookRewardCommand(int RewardIndex, int CollectionIndex) : CodecCommand<ClaimDecoStickerBookRewardCommand>, ICommandCodecProvider<ClaimDecoStickerBookRewardCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimDecoStickerBookRewardCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.RewardIndex,
            Codecs.VarInt,
            static command => command.CollectionIndex,
            static (rewardIndex, collectionIndex) => new ClaimDecoStickerBookRewardCommand(rewardIndex, collectionIndex)
        ),
        CommandLayout.MetadataFirst
    );
}
