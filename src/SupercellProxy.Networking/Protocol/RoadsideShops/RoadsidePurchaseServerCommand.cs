using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>
/// Completes the buyer's side of a roadside purchase: the buyer pays the coins and gets the goods, unless the barn or silo has no
/// room, coins are short, or the daily limit on collection tools is reached.
/// </summary>
/// <param name="BuyerId">Home of the player who bought the stand; other homes ignore the command.</param>
/// <param name="ShopOwnerId">Home that owns the shop.</param>
/// <param name="ContextId">Third home or account id of the command; the client does not read it and its meaning is not identified.</param>
/// <param name="ItemGlobalId">Global id of the bought goods.</param>
/// <param name="SlotIndex">Position of the bought stand in the shop.</param>
/// <param name="Quantity">How many goods the buyer gets.</param>
/// <param name="Price">Coins the buyer pays.</param>
public sealed record RoadsidePurchaseServerCommand(
    [property: JsonPropertyName("BuyerId")] LongId BuyerId,
    [property: JsonPropertyName("ShopOwnerId")] LongId ShopOwnerId,
    [property: JsonPropertyName("ContextId")] LongId ContextId,
    [property: JsonPropertyName("ItemGlobalId")] int ItemGlobalId,
    int SlotIndex,
    int Quantity,
    int Price
) : CodecServerCommand<RoadsidePurchaseServerCommand>, ICommandCodecProvider<RoadsidePurchaseServerCommand>
{
    /// <summary>How many collection tools a player may buy from roadside shops per day.</summary>
    public const int DailyCollectionToolLimit = 80;

    /// <inheritdoc />
    public static CommandCodec<RoadsidePurchaseServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.LongId,
            static command => command.BuyerId,
            Codecs.LongId,
            static command => command.ShopOwnerId,
            Codecs.LongId,
            static command => command.ContextId,
            Codecs.Int32,
            static command => command.ItemGlobalId,
            Codecs.Int32,
            static command => command.SlotIndex,
            Codecs.Int32,
            static command => command.Quantity,
            Codecs.Int32,
            static command => command.Price,
            static (buyer, owner, context, item, slot, quantity, price) => new RoadsidePurchaseServerCommand(buyer, owner, context, item, slot, quantity, price)
        )
    );
}
