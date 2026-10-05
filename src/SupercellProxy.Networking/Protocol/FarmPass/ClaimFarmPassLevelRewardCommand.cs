using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>Claims the level's free or premium Farm Pass level reward group.</summary>
/// <param name="LevelIndex">Zero-based level position in the season.</param>
/// <param name="LevelId">Cumulative Farm Points threshold identifying the level.</param>
/// <param name="Premium">Whether the premium reward group is claimed.</param>
public sealed record ClaimFarmPassLevelRewardCommand(int LevelIndex, int LevelId, bool Premium) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.ClaimFarmPassLevelRewardCommandType;

    /// <summary>Decodes a Farm Pass level reward claim.</summary>
    public static ClaimFarmPassLevelRewardCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int level = stream.ReadVarInt();
        int levelId = stream.ReadVarInt();
        bool premium = stream.ReadBoolean();

        return new ClaimFarmPassLevelRewardCommand(level, levelId, premium);
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(LevelIndex);
        stream.WriteVarInt(LevelId);
        stream.WriteBoolean(Premium);
    }
}
