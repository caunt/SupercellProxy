using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Orders;

/// <summary>
/// Defines the Mark Truck Orders Seen Command contract.
/// </summary>
/// <summary>
/// Defines the Order Table Global Id contract.
/// </summary>
public sealed record MarkTruckOrdersSeenCommand([property: System.Text.Json.Serialization.JsonPropertyName("OrderTableGlobalId")] int OrderTableGlobalId) : Command
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.MarkTruckOrdersSeenCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static MarkTruckOrdersSeenCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int table = stream.ReadVarInt();

        return new MarkTruckOrdersSeenCommand(table);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(OrderTableGlobalId);
    }
}
