using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.RoadsideShops.Sales;

/// <summary>Reports a sale from a roadside shop stand; the client does not act on it.</summary>
/// <param name="OwnerHomeId">Home of the seller.</param>
/// <param name="BuyerHomeId">Home of the player who bought the stand.</param>
/// <param name="StandIndex">Position of the sold stand in the seller's shop.</param>
/// <param name="ItemGlobalId">Global id of the sold goods.</param>
/// <param name="Amount">How many goods were on the stand.</param>
/// <param name="Price">Coin price of the stand.</param>
public sealed record RoadsideShopSaleServerCommand(LongId OwnerHomeId, LongId BuyerHomeId, int StandIndex, int ItemGlobalId, int Amount, int Price) : CodecServerCommand<RoadsideShopSaleServerCommand>, ICommandCodecProvider<RoadsideShopSaleServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RoadsideShopSaleServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.LongId,
            static command => command.OwnerHomeId,
            Codecs.LongId,
            static command => command.BuyerHomeId,
            Codecs.Int32,
            static command => command.StandIndex,
            Codecs.Int32,
            static command => command.ItemGlobalId,
            Codecs.Int32,
            static command => command.Amount,
            Codecs.Int32,
            static command => command.Price,
            static (ownerHomeId, buyerHomeId, standIndex, itemGlobalId, amount, price) => new RoadsideShopSaleServerCommand(ownerHomeId, buyerHomeId, standIndex, itemGlobalId, amount, price)
        )
    );
}
