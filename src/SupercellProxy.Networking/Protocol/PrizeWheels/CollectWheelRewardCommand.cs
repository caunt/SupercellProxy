using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.PrizeWheels;

/// <summary>
/// Defines the Collect Wheel Reward Command contract.
/// </summary>
/// <summary>
/// Defines the Client Presentation contract.
/// </summary>
public sealed record CollectWheelRewardCommand(bool ClientPresentation) : Command
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.CollectWheelRewardCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static CollectWheelRewardCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        bool presentation = stream.ReadBoolean();

        return new CollectWheelRewardCommand(presentation);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteBoolean(ClientPresentation);
    }
}
