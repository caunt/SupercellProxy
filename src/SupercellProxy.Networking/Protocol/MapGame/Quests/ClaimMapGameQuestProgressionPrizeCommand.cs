using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.MapGame.Quests;

/// <summary>Claims the mystery-box prize for one completed Valley quest-progression milestone.</summary>
public sealed record ClaimMapGameQuestProgressionPrizeCommand(
    int ProgressionPrizeGlobalIdentifier,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.ClaimMapGameQuestProgressionPrizeCommandType;

    /// <summary>Decodes the progression data reference followed by the base command fields.</summary>
    public static ClaimMapGameQuestProgressionPrizeCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int identifier = stream.ReadVariableInt();
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new ClaimMapGameQuestProgressionPrizeCommand(identifier, fields.ExecutionPhaseCounter, fields.DebugData0, fields.DebugData1);
    }

    /// <inheritdoc />
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVariableInt(ProgressionPrizeGlobalIdentifier);
        EncodeCommand(stream, environment);
    }
}
