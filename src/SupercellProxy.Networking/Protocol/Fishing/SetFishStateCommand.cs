using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Fishing;

/// <summary>Moves one fishing-area fish to the selected runtime state.</summary>
public sealed record SetFishStateCommand(
    [property: System.Text.Json.Serialization.JsonPropertyName("FishGlobalId")] int FishGlobalIdentifier,
    int State,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.SetFishStateCommandType;

    /// <summary>Decodes a value from the supplied protocol payload.</summary>
    public static SetFishStateCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int fishGlobalIdentifier = stream.ReadVariableInt();
        int state = stream.ReadVariableInt();
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new SetFishStateCommand(fishGlobalIdentifier, state, fields.ExecutionPhaseCounter, fields.DebugData0, fields.DebugData1);
    }

    /// <inheritdoc />
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVariableInt(FishGlobalIdentifier);
        stream.WriteVariableInt(State);
        EncodeCommand(stream, environment);
    }
}
