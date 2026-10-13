using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.RoadsideShops.Sales;

/// <summary>Reports a sale from a roadside shop stand.</summary>
/// <param name="OwnerHomeId">The owner home id.</param>
/// <param name="BuyerHomeId">The buyer home id.</param>
/// <param name="StandIndex">The stand index.</param>
/// <param name="ItemGlobalId">The item data id.</param>
/// <param name="Amount">The amount.</param>
/// <param name="Price">The price.</param>
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
