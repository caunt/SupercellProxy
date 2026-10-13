using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>
/// Defines the Roadside Purchase Server Command contract.
/// </summary>
/// <summary>
/// Defines the Buyer Id contract.
/// </summary>
/// <summary>
/// Defines the Shop Owner Id contract.
/// </summary>
/// <summary>
/// Defines the Context Id contract.
/// </summary>
/// <summary>
/// Defines the Item Global Id contract.
/// </summary>
/// <summary>
/// Defines the Slot Index contract.
/// </summary>
/// <summary>
/// Defines the Quantity contract.
/// </summary>
/// <summary>
/// Defines the Price contract.
/// </summary>
public sealed record RoadsidePurchaseServerCommand(
    [property: System.Text.Json.Serialization.JsonPropertyName("BuyerId")] LongId BuyerId,
    [property: System.Text.Json.Serialization.JsonPropertyName("ShopOwnerId")] LongId ShopOwnerId,
    [property: System.Text.Json.Serialization.JsonPropertyName("ContextId")] LongId ContextId,
    [property: System.Text.Json.Serialization.JsonPropertyName("ItemGlobalId")] int ItemGlobalId,
    int SlotIndex,
    int Quantity,
    int Price
) : CodecServerCommand<RoadsidePurchaseServerCommand>, ICommandCodecProvider<RoadsidePurchaseServerCommand>
{
    /// <summary>
    /// Provides the Daily Collection Tool Limit value or operation.
    /// </summary>
    public const int DailyCollectionToolLimit = 80;

    /// <inheritdoc />
    public static CommandCodec<RoadsidePurchaseServerCommand> Codec { get; } = Codecs.Command(
        type: 243,
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
