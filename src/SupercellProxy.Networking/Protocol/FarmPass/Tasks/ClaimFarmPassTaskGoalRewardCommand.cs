using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass.Tasks;

/// <summary>Claims the reward for reaching a Farm Pass task goal.</summary>
/// <param name="GoalIndex">The reached goal.</param>
public sealed record ClaimFarmPassTaskGoalRewardCommand(int GoalIndex) : CodecCommand<ClaimFarmPassTaskGoalRewardCommand>, ICommandCodecProvider<ClaimFarmPassTaskGoalRewardCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimFarmPassTaskGoalRewardCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.GoalIndex, static (goalIndex) => new ClaimFarmPassTaskGoalRewardCommand(goalIndex))
    );
}
