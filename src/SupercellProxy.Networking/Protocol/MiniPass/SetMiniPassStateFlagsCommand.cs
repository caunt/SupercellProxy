using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.MiniPass;

/// <summary>Sets selected state flags on one Mini Pass instance.</summary>
public sealed record SetMiniPassStateFlagsCommand(int Flags, bool SecondaryPass, int ExecutionPhaseCounter = -1, CommandData? DebugData0 = null, CommandData? DebugData1 = null) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.SetMiniPassStateFlagsCommandType;

    /// <summary>Decodes the base command followed by the flag mask and pass selector.</summary>
    public static SetMiniPassStateFlagsCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new SetMiniPassStateFlagsCommand(stream.ReadVariableInt(), stream.ReadBoolean(), fields.ExecutionPhaseCounter, fields.DebugData0, fields.DebugData1);
    }

    /// <inheritdoc />
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        EncodeCommand(stream, environment);
        stream.WriteVariableInt(Flags);
        stream.WriteBoolean(SecondaryPass);
    }
}
