using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass.Rewards;

/// <summary>Selects a reward of the Farm Pass reward road.</summary>
/// <param name="RewardIndex">The reward index.</param>
public sealed record SelectFarmPassRewardCommand(int RewardIndex) : CodecCommand<SelectFarmPassRewardCommand>, ICommandCodecProvider<SelectFarmPassRewardCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SelectFarmPassRewardCommand> Codec { get; } = Codecs.Command(
        type: 335,
        Codecs.Composite(Codecs.VarInt, static command => command.RewardIndex, static (rewardIndex) => new SelectFarmPassRewardCommand(rewardIndex))
    );
}
