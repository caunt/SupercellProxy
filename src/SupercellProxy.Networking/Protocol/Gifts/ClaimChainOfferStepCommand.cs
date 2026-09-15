using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Gifts;

/// <summary>Claims one reward step from a chain offer.</summary>
public sealed record ClaimChainOfferStepCommand(
    int StepIndex,
    [property: System.Text.Json.Serialization.JsonPropertyName("EventId")] int EventIdentifier,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.ClaimChainOfferStepCommandType;

    /// <summary>Decodes a chain-offer step claim.</summary>
    public static ClaimChainOfferStepCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new ClaimChainOfferStepCommand(stream.ReadVariableInt(), stream.ReadVariableInt(), fields.ExecutionPhaseCounter, fields.DebugData0, fields.DebugData1);
    }

    /// <inheritdoc />
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        EncodeCommand(stream, environment);
        stream.WriteVariableInt(StepIndex);
        stream.WriteVariableInt(EventIdentifier);
    }
}
