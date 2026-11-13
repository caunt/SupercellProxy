using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>A server refusal containing the attempted listing and the client's material-purchase limit.</summary>
/// <param name="BuyerId">Home of the player whose purchase was refused, the first field on the wire.</param>
/// <param name="ShopOwnerId">Home of the seller whose roadside shop the buyer tried to buy from.</param>
/// <param name="ItemGlobalId">Global id of the goods on the stand the buyer tried to buy, written as a big-endian 32-bit integer rather than a var-int.</param>
/// <param name="SlotIndex">Zero-based position of that stand in the seller's shop, as a big-endian 32-bit integer.</param>
/// <param name="Quantity">How many goods the stand held when the buyer tried to buy it, as a big-endian 32-bit integer.</param>
/// <param name="Price">Coin price of the whole stand as the buyer saw it, as a big-endian 32-bit integer.</param>
/// <param name="DailyCollectionToolLimit">How many collection tools the buyer may buy from roadside shops per day, as a var-int and the last field on the wire.</param>
public sealed record RoadsidePurchaseRejectedServerCommand(LongId BuyerId, LongId ShopOwnerId, int ItemGlobalId, int SlotIndex, int Quantity, int Price, int DailyCollectionToolLimit) : CodecServerCommand<RoadsidePurchaseRejectedServerCommand>, ICommandCodecProvider<RoadsidePurchaseRejectedServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RoadsidePurchaseRejectedServerCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.LongId,
        static command => command.BuyerId,
        Codecs.LongId,
        static command => command.ShopOwnerId,
        Codecs.BigEndianInt32,
        static command => command.ItemGlobalId,
        Codecs.BigEndianInt32,
        static command => command.SlotIndex,
        Codecs.BigEndianInt32,
        static command => command.Quantity,
        Codecs.BigEndianInt32,
        static command => command.Price,
        Codecs.VarInt,
        static command => command.DailyCollectionToolLimit,
        static (buyer, owner, item, slot, quantity, price, limit) => new RoadsidePurchaseRejectedServerCommand(buyer, owner, item, slot, quantity, price, limit)));
}
