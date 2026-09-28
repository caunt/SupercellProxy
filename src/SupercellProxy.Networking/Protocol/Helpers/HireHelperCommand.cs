using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Helpers;

/// Hires one farm helper at a configured duration tier.
public sealed record HireHelperCommand(
    int HelperIndex,
    int HireTierIndex,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.HireHelperCommandType;

    /// Decodes the helper and tier before the shared command fields.
    public static HireHelperCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int helperIndex = stream.ReadVariableInt();
        int tierIndex = stream.ReadVariableInt();
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new HireHelperCommand(helperIndex, tierIndex, fields.ExecutionPhaseCounter, fields.DebugData0, fields.DebugData1);
    }

    /// <inheritdoc />
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVariableInt(HelperIndex);
        stream.WriteVariableInt(HireTierIndex);
        EncodeCommand(stream, environment);
    }
}
