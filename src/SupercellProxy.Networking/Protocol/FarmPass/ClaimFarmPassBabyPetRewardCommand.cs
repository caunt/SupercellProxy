using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>Claims the level's free or premium Farm Pass baby-pet reward group.</summary>
/// <param name="LevelIndex">Zero-based index of the level in the joined season's level list.</param>
/// <param name="Premium"><see langword="true"/> to claim the premium baby pets; <see langword="false"/> to claim the free ones.</param>
public sealed record ClaimFarmPassBabyPetRewardCommand(int LevelIndex, bool Premium) : CodecCommand<ClaimFarmPassBabyPetRewardCommand>, ICommandCodecProvider<ClaimFarmPassBabyPetRewardCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimFarmPassBabyPetRewardCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt,
        static command => command.LevelIndex,
        Codecs.Boolean,
        static command => command.Premium,
        static (level, premium) => new ClaimFarmPassBabyPetRewardCommand(level, premium)));
}
