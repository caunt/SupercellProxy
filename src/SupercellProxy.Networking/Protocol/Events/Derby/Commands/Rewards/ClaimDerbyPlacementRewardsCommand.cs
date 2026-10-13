using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands.Rewards;

/// <summary>Claims the previous derby's podium rewards and finishes its presentation.</summary>
public sealed record ClaimDerbyPlacementRewardsCommand : CodecCommand<ClaimDerbyPlacementRewardsCommand>, ICommandCodecProvider<ClaimDerbyPlacementRewardsCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimDerbyPlacementRewardsCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new ClaimDerbyPlacementRewardsCommand()), CommandLayout.MetadataFirst);
}
