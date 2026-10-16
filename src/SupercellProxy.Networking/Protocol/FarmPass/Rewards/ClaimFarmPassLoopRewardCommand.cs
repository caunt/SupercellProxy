using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass.Rewards;

/// <summary>Claims the repeating reward that season points earn after the last Farm Pass level.</summary>
/// <param name="UseDiamonds">Whether the claim spends diamonds; the client logic does not port this command yet.</param>
public sealed record ClaimFarmPassLoopRewardCommand(bool UseDiamonds) : CodecCommand<ClaimFarmPassLoopRewardCommand>, ICommandCodecProvider<ClaimFarmPassLoopRewardCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimFarmPassLoopRewardCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.Boolean, static command => command.UseDiamonds, static useDiamonds => new ClaimFarmPassLoopRewardCommand(useDiamonds)),
        CommandLayout.MetadataFirst
    );
}
