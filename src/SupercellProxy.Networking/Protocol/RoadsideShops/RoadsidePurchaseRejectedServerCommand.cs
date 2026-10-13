using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>A server refusal containing the attempted listing and the client's material-purchase limit.</summary>
public sealed record RoadsidePurchaseRejectedServerCommand(LongId BuyerId, LongId ShopOwnerId, int ItemGlobalId, int SlotIndex, int Quantity, int Price, int DailyCollectionToolLimit) : CodecServerCommand<RoadsidePurchaseRejectedServerCommand>, ICommandCodecProvider<RoadsidePurchaseRejectedServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RoadsidePurchaseRejectedServerCommand> Codec { get; } = Codecs.Command(
        type: 309,
        Codecs.Composite(
            Codecs.LongId,
            static command => command.BuyerId,
            Codecs.LongId,
            static command => command.ShopOwnerId,
            Codecs.Int32,
            static command => command.ItemGlobalId,
            Codecs.Int32,
            static command => command.SlotIndex,
            Codecs.Int32,
            static command => command.Quantity,
            Codecs.Int32,
            static command => command.Price,
            Codecs.VarInt,
            static command => command.DailyCollectionToolLimit,
            static (buyer, owner, item, slot, quantity, price, limit) => new RoadsidePurchaseRejectedServerCommand(buyer, owner, item, slot, quantity, price, limit)
        )
    );
}
