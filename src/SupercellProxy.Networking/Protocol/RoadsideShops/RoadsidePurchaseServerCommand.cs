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
    [property: System.Text.Json.Serialization.JsonPropertyName("BuyerId")] LongIdentifier BuyerIdentifier,
    [property: System.Text.Json.Serialization.JsonPropertyName("ShopOwnerId")] LongIdentifier ShopOwnerIdentifier,
    [property: System.Text.Json.Serialization.JsonPropertyName("ContextId")] LongIdentifier ContextIdentifier,
    [property: System.Text.Json.Serialization.JsonPropertyName("ItemGlobalId")] int ItemGlobalIdentifier,
    int SlotIndex,
    int Quantity,
    int Price,
    int ServerCommandIdentifier,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : ServerCommand(ServerCommandIdentifier, ExecutionPhaseCounter, DebugData0, DebugData1)
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
        LongIdentifier buyer = stream.ReadLongIdentifier();
        LongIdentifier owner = stream.ReadLongIdentifier();
        LongIdentifier context = stream.ReadLongIdentifier();
        int item = stream.ReadInt32();
        int slot = stream.ReadInt32();
        int quantity = stream.ReadInt32();
        int price = stream.ReadInt32();
        (int serverCommandIdentifier, (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) commandFields) = DecodeServerCommand(stream, environment);

        return new(
            buyer,
            owner,
            context,
            item,
            slot,
            quantity,
            price,
            serverCommandIdentifier,
            commandFields.ExecutionPhaseCounter,
            commandFields.DebugData0,
            commandFields.DebugData1
        );
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteLongIdentifier(BuyerIdentifier);
        stream.WriteLongIdentifier(ShopOwnerIdentifier);
        stream.WriteLongIdentifier(ContextIdentifier);
        stream.WriteInt32(ItemGlobalIdentifier);
        stream.WriteInt32(SlotIndex);
        stream.WriteInt32(Quantity);
        stream.WriteInt32(Price);
        EncodeServerCommand(stream, environment);
    }
}
