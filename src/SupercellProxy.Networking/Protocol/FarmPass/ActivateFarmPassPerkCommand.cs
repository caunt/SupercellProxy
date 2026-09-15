using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>Activates a claimed Farm Pass perk, optionally for one parameter data row.</summary>
public sealed record ActivateFarmPassPerkCommand(
    int PerkDataGlobalIdentifier,
    int ParameterDataGlobalIdentifier,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.ActivateFarmPassPerkCommandType;

    /// <summary>Decodes a Farm Pass perk activation command.</summary>
    public static ActivateFarmPassPerkCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int perk = stream.ReadVariableInt();
        int parameter = stream.ReadVariableInt();
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new ActivateFarmPassPerkCommand(perk, parameter, fields.ExecutionPhaseCounter, fields.DebugData0, fields.DebugData1);
    }

    /// <inheritdoc />
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVariableInt(PerkDataGlobalIdentifier);
        stream.WriteVariableInt(ParameterDataGlobalIdentifier);
        EncodeCommand(stream, environment);
    }
}
