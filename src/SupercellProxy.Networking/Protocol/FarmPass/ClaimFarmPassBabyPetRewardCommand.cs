using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>Claims the level's free or premium Farm Pass baby-pet reward group.</summary>
public sealed record ClaimFarmPassBabyPetRewardCommand(int LevelIndex, bool Premium, int ExecutionPhaseCounter = -1, CommandData? DebugData0 = null, CommandData? DebugData1 = null) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.ClaimFarmPassBabyPetRewardCommandType;

    /// <summary>Decodes a Farm Pass baby-pet reward claim.</summary>
    public static ClaimFarmPassBabyPetRewardCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int level = stream.ReadVariableInt();
        bool premium = stream.ReadBoolean();
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new ClaimFarmPassBabyPetRewardCommand(level, premium, fields.ExecutionPhaseCounter, fields.DebugData0, fields.DebugData1);
    }

    /// <inheritdoc />
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVariableInt(LevelIndex);
        stream.WriteBoolean(Premium);
        EncodeCommand(stream, environment);
    }
}
