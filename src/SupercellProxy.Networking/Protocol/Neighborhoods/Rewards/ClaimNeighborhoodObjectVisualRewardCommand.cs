using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Rewards;

/// <summary>Claims the decoration for a one-based Neighborhood Nurture visual milestone.</summary>
/// <param name="VisualLevel">One-based milestone number, written as a VarInt; level 1 is the event's first visual level.</param>
public sealed record ClaimNeighborhoodObjectVisualRewardCommand(int VisualLevel) : CodecCommand<ClaimNeighborhoodObjectVisualRewardCommand>, ICommandCodecProvider<ClaimNeighborhoodObjectVisualRewardCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimNeighborhoodObjectVisualRewardCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt,
            static command => command.VisualLevel,
            static visualLevel => new ClaimNeighborhoodObjectVisualRewardCommand(visualLevel)),
        CommandLayout.MetadataFirst);
}
