using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>Completes a finished Neighborhood Object task in the selected active slot.</summary>
public sealed record CompleteNeighborhoodObjectTaskCommand(int TaskSlotIndex, int ExecutionPhaseCounter = -1, CommandData? DebugData0 = null, CommandData? DebugData1 = null) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.CompleteNeighborhoodObjectTaskCommandType;

    /// <summary>Decodes the base command followed by the task slot index.</summary>
    public static CompleteNeighborhoodObjectTaskCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new CompleteNeighborhoodObjectTaskCommand(stream.ReadVariableInt(), fields.ExecutionPhaseCounter, fields.DebugData0, fields.DebugData1);
    }

    /// <inheritdoc />
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        EncodeCommand(stream, environment);
        stream.WriteVariableInt(TaskSlotIndex);
    }
}
