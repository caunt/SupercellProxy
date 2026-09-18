using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>Claims the level's free or premium Farm Pass level reward group.</summary>
/// <param name="LevelIndex">Zero-based level position in the season.</param>
/// <param name="LevelIdentifier">Cumulative Farm Points threshold identifying the level.</param>
/// <param name="Premium">Whether the premium reward group is claimed.</param>
/// <param name="ExecutionPhaseCounter">Execution phase the command runs in.</param>
/// <param name="DebugData0">Optional development debug data.</param>
/// <param name="DebugData1">Optional development debug data.</param>
public sealed record ClaimFarmPassLevelRewardCommand(
    int LevelIndex,
    int LevelIdentifier,
    bool Premium,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.ClaimFarmPassLevelRewardCommandType;

    /// <summary>Decodes a Farm Pass level reward claim.</summary>
    public static ClaimFarmPassLevelRewardCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int level = stream.ReadVariableInt();
        int levelIdentifier = stream.ReadVariableInt();
        bool premium = stream.ReadBoolean();
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new ClaimFarmPassLevelRewardCommand(level, levelIdentifier, premium, fields.ExecutionPhaseCounter, fields.DebugData0, fields.DebugData1);
    }

    /// <inheritdoc />
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVariableInt(LevelIndex);
        stream.WriteVariableInt(LevelIdentifier);
        stream.WriteBoolean(Premium);
        EncodeCommand(stream, environment);
    }
}
