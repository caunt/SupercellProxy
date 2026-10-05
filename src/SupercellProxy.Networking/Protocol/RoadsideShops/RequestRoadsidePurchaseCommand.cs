using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>
/// Defines the Request Roadside Purchase Command contract.
/// </summary>
/// <summary>
/// Defines the Slot Index contract.
/// </summary>
/// <summary>
/// Defines the Item Global Id contract.
/// </summary>
public sealed record RequestRoadsidePurchaseCommand(int SlotIndex, [property: System.Text.Json.Serialization.JsonPropertyName("ItemGlobalId")] int ItemGlobalId) : Command
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.RequestRoadsidePurchaseCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static RequestRoadsidePurchaseCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new RequestRoadsidePurchaseCommand(stream.ReadVarInt(), stream.ReadVarInt());
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(SlotIndex);
        stream.WriteVarInt(ItemGlobalId);
    }
}
