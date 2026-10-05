using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


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
) : ServerCommand
{
    /// <summary>
    /// Provides the Daily Collection Tool Limit value or operation.
    /// </summary>
    public const int DailyCollectionToolLimit = 80;

    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.RoadsidePurchaseServerCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static RoadsidePurchaseServerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        LongId buyer = stream.ReadLongId();
        LongId owner = stream.ReadLongId();
        LongId context = stream.ReadLongId();
        int item = stream.ReadInt32();
        int slot = stream.ReadInt32();
        int quantity = stream.ReadInt32();
        int price = stream.ReadInt32();


        return new(buyer, owner, context, item, slot, quantity, price);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteLongId(BuyerId);
        stream.WriteLongId(ShopOwnerId);
        stream.WriteLongId(ContextId);
        stream.WriteInt32(ItemGlobalId);
        stream.WriteInt32(SlotIndex);
        stream.WriteInt32(Quantity);
        stream.WriteInt32(Price);
    }
}
