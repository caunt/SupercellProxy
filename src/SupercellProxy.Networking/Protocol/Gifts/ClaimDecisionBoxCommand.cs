using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Gifts;

/// <summary>
/// Defines the Claim Decision Box Command contract.
/// </summary>
/// <summary>
/// Defines the Choice Index contract.
/// </summary>
public sealed record ClaimDecisionBoxCommand(int ChoiceIndex) : Command
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.ClaimDecisionBoxCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static ClaimDecisionBoxCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new ClaimDecisionBoxCommand(stream.ReadVarInt());
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(ChoiceIndex);
    }
}
