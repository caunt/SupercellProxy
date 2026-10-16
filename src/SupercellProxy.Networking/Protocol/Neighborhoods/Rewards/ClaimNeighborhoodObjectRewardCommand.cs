using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Rewards;

/// <summary>Claims a free reward tier from the neighborhood event.</summary>
public sealed record ClaimNeighborhoodObjectRewardCommand(int TierIndex) : CodecCommand<ClaimNeighborhoodObjectRewardCommand>, ICommandCodecProvider<ClaimNeighborhoodObjectRewardCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimNeighborhoodObjectRewardCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.TierIndex, static tierIndex => new ClaimNeighborhoodObjectRewardCommand(tierIndex)),
        CommandLayout.MetadataFirst
    );
}
