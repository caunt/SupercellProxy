using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Membership;

/// <summary>Confirms removal of the player from the specified neighborhood.</summary>
public sealed record LeaveNeighborhoodServerCommand(LongId NeighborhoodId) : CodecServerCommand<LeaveNeighborhoodServerCommand>, ICommandCodecProvider<LeaveNeighborhoodServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<LeaveNeighborhoodServerCommand> Codec { get; } = Codecs.Command(
        type: 135,
        Codecs.Composite(
            Codecs.LongId,
            static command => command.NeighborhoodId,
            static (neighborhoodId) => new LeaveNeighborhoodServerCommand(neighborhoodId)
        )
    );
}
