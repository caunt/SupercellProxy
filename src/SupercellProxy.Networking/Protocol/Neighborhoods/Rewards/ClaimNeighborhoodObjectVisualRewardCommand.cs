using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>Claims the decoration for a one-based Neighborhood Nurture visual milestone.</summary>
public sealed record ClaimNeighborhoodObjectVisualRewardCommand(int VisualLevel) : CodecCommand<ClaimNeighborhoodObjectVisualRewardCommand>, ICommandCodecProvider<ClaimNeighborhoodObjectVisualRewardCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimNeighborhoodObjectVisualRewardCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.VisualLevel,
            static (visualLevel) => new ClaimNeighborhoodObjectVisualRewardCommand(visualLevel)
        ),
        CommandLayout.MetadataFirst
    );
}
