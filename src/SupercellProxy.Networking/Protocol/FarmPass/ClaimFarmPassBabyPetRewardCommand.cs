using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>Claims the level's free or premium Farm Pass baby-pet reward group.</summary>
public sealed record ClaimFarmPassBabyPetRewardCommand(int LevelIndex, bool Premium) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.ClaimFarmPassBabyPetRewardCommandType;

    /// <summary>Decodes a Farm Pass baby-pet reward claim.</summary>
    public static ClaimFarmPassBabyPetRewardCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int level = stream.ReadVarInt();
        bool premium = stream.ReadBoolean();

        return new ClaimFarmPassBabyPetRewardCommand(level, premium);
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(LevelIndex);
        stream.WriteBoolean(Premium);
    }
}
