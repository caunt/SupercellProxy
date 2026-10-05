using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Orders;

/// <summary>
/// Defines the Collect Truck Delivery Rewards Command contract.
/// </summary>
public sealed record CollectTruckDeliveryRewardsCommand() : Command
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.CollectTruckDeliveryRewardsCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static CollectTruckDeliveryRewardsCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        return new CollectTruckDeliveryRewardsCommand();
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
    }
}
