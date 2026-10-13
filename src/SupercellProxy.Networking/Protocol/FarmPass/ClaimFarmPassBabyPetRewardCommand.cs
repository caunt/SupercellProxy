using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>Claims the level's free or premium Farm Pass baby-pet reward group.</summary>
public sealed record ClaimFarmPassBabyPetRewardCommand(int LevelIndex, bool Premium) : CodecCommand<ClaimFarmPassBabyPetRewardCommand>, ICommandCodecProvider<ClaimFarmPassBabyPetRewardCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimFarmPassBabyPetRewardCommand> Codec { get; } = Codecs.Command(
        type: 346,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.LevelIndex,
            Codecs.Boolean,
            static command => command.Premium,
            static (level, premium) => new ClaimFarmPassBabyPetRewardCommand(level, premium)
        )
    );
}
