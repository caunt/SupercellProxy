using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass.Rewards;

/// <summary>Claims a Farm Pass loop reward.</summary>
/// <param name="UseDiamonds">The use diamonds.</param>
public sealed record ClaimFarmPassLoopRewardCommand(bool UseDiamonds) : CodecCommand<ClaimFarmPassLoopRewardCommand>, ICommandCodecProvider<ClaimFarmPassLoopRewardCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimFarmPassLoopRewardCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.Boolean, static command => command.UseDiamonds, static (useDiamonds) => new ClaimFarmPassLoopRewardCommand(useDiamonds)),
        CommandLayout.MetadataFirst
    );
}
