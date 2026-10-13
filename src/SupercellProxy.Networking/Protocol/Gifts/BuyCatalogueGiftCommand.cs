using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Gifts;

/// <summary>Buys one gift offered by the ordinary gift catalogue.</summary>
public sealed record BuyCatalogueGiftCommand(int GiftDataGlobalId, int CatalogueSlotDataGlobalId, int RewardSelectionValue) : CodecCommand<BuyCatalogueGiftCommand>, ICommandCodecProvider<BuyCatalogueGiftCommand>
{
    /// <inheritdoc />
    public static CommandCodec<BuyCatalogueGiftCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.GiftDataGlobalId,
            Codecs.VarInt,
            static command => command.CatalogueSlotDataGlobalId,
            Codecs.VarInt,
            static command => command.RewardSelectionValue,
            static (gift, slot, rewardSelection) => new BuyCatalogueGiftCommand(gift, slot, rewardSelection)
        )
    );
}
