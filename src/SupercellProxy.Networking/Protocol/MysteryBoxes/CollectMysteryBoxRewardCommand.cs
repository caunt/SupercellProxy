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
public sealed record CollectMysteryBoxRewardCommand(
    [property: System.Text.Json.Serialization.JsonPropertyName("BoxGlobalId")] int BoxGlobalIdentifier,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
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
        int box = stream.ReadVariableInt();
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new CollectMysteryBoxRewardCommand(box, fields.ExecutionPhaseCounter, fields.DebugData0, fields.DebugData1);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVariableInt(BoxGlobalIdentifier);
        EncodeCommand(stream, environment);
    }
}
