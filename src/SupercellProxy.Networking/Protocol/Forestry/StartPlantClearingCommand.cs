using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Forestry;

/// <summary>Marks an exhausted plant for clearing, optionally buying its missing tool with coins.</summary>
public sealed record StartPlantClearingCommand(
    int PlantIdentifier,
    bool BuyMissingTool,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.StartPlantClearingCommandType;

    /// <summary>Decodes the native plant identifier and coin-purchase flag before base command fields.</summary>
    public static StartPlantClearingCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int identifier = stream.ReadVariableInt();
        bool buy = stream.ReadBoolean();
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new(identifier, buy, fields.ExecutionPhaseCounter, fields.DebugData0, fields.DebugData1);
    }

    /// <inheritdoc />
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVariableInt(PlantIdentifier);
        stream.WriteBoolean(BuyMissingTool);
        EncodeCommand(stream, environment);
    }
}
