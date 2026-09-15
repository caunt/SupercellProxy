using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Forestry;

/// <summary>Collects one fruit from a fruit tree or berry bush.</summary>
public sealed record CollectFruitCommand(
    int FruitIndex,
    [property: System.Text.Json.Serialization.JsonPropertyName("FruitTreeGlobalId")] int FruitTreeGlobalIdentifier,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.CollectFruitCommandType;

    /// <summary>Decodes a value from the supplied protocol payload.</summary>
    public static CollectFruitCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new CollectFruitCommand(stream.ReadVariableInt(), stream.ReadVariableInt(), fields.ExecutionPhaseCounter, fields.DebugData0, fields.DebugData1);
    }

    /// <inheritdoc />
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        EncodeCommand(stream, environment);
        stream.WriteVariableInt(FruitIndex);
        stream.WriteVariableInt(FruitTreeGlobalIdentifier);
    }
}
