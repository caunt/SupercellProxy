using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Creatures;

/// <summary>Starts catching an existing seasonal creature.</summary>
public sealed record CatchCreatureCommand(int CreatureGlobalIdentifier, int ExecutionPhaseCounter = -1, CommandData? DebugData0 = null, CommandData? DebugData1 = null) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <summary>Gets the native command identifier.</summary>
    public override int Type => CommandRegistry.CatchCreatureCommandType;

    /// <summary>Decodes the command header and creature instance identifier.</summary>
    public static CatchCreatureCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new(stream.ReadVariableInt(), fields.ExecutionPhaseCounter, fields.DebugData0, fields.DebugData1);
    }

    /// <summary>Encodes the command header and creature instance identifier.</summary>
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        EncodeCommand(stream, environment);
        stream.WriteVariableInt(CreatureGlobalIdentifier);
    }
}
