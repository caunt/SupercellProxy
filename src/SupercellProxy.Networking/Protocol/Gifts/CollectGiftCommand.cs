using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Gifts;

/// <summary>
/// Defines the Collect Gift Command contract.
/// </summary>
/// <summary>
/// Defines the Gift Global Id contract.
/// </summary>
public sealed record CollectGiftCommand(
    [property: System.Text.Json.Serialization.JsonPropertyName("GiftGlobalId")] int GiftGlobalIdentifier,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.CollectGiftCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static CollectGiftCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int gift = stream.ReadVariableInt();
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new CollectGiftCommand(gift, fields.ExecutionPhaseCounter, fields.DebugData0, fields.DebugData1);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVariableInt(GiftGlobalIdentifier);
        EncodeCommand(stream, environment);
    }
}
