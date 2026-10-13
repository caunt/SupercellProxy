using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>Claims the level's free or premium Farm Pass level reward group.</summary>
/// <param name="LevelIndex">Zero-based level position in the season.</param>
/// <param name="LevelId">Cumulative Farm Points threshold identifying the level.</param>
/// <param name="Premium">Whether the premium reward group is claimed.</param>
public sealed record ClaimFarmPassLevelRewardCommand(int LevelIndex, int LevelId, bool Premium) : CodecCommand<ClaimFarmPassLevelRewardCommand>, ICommandCodecProvider<ClaimFarmPassLevelRewardCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimFarmPassLevelRewardCommand> Codec { get; } = Codecs.Command(
        type: 336,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.LevelIndex,
            Codecs.VarInt,
            static command => command.LevelId,
            Codecs.Boolean,
            static command => command.Premium,
            static (level, levelId, premium) => new ClaimFarmPassLevelRewardCommand(level, levelId, premium)
        )
    );
}
