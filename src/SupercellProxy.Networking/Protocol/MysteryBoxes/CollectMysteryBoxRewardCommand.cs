using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MysteryBoxes;

/// <summary>
/// Defines the Collect Mystery Box Reward Command contract.
/// </summary>
/// <summary>
/// Defines the Box Global Id contract.
/// </summary>
public sealed record CollectMysteryBoxRewardCommand([property: System.Text.Json.Serialization.JsonPropertyName("BoxGlobalId")] int BoxGlobalId) : Command
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.CollectMysteryBoxRewardCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static CollectMysteryBoxRewardCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int box = stream.ReadVarInt();

        return new CollectMysteryBoxRewardCommand(box);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(BoxGlobalId);
    }
}
