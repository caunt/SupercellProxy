using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Gifts;

/// <summary>Buys one gift offered by the ordinary gift catalogue.</summary>
/// <param name="GiftDataGlobalId">Global id of the gift's row in the gifts table, written first as a VarInt.</param>
/// <param name="CatalogueSlotDataGlobalId">Global id of the catalogue gifts row that offers the gift, written as a VarInt after the gift id.</param>
/// <param name="RewardSelectionValue">Reward selector written last as a VarInt; 0 is the usual value.</param>
public sealed record BuyCatalogueGiftCommand(int GiftDataGlobalId, int CatalogueSlotDataGlobalId, int RewardSelectionValue) : CodecCommand<BuyCatalogueGiftCommand>, ICommandCodecProvider<BuyCatalogueGiftCommand>
{
    /// <inheritdoc />
    public static CommandCodec<BuyCatalogueGiftCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt,
        static command => command.GiftDataGlobalId,
        Codecs.VarInt,
        static command => command.CatalogueSlotDataGlobalId,
        Codecs.VarInt,
        static command => command.RewardSelectionValue,
        static (gift, slot, rewardSelection) => new BuyCatalogueGiftCommand(gift, slot, rewardSelection)));
}
