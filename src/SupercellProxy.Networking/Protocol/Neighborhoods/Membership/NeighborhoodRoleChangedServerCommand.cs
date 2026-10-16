using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Membership;

/// <summary>Sets the avatar's role inside its neighborhood.</summary>
/// <param name="NeighborhoodId">Neighbourhood the role belongs to.</param>
/// <param name="Role">The new member role.</param>
public sealed record NeighborhoodRoleChangedServerCommand(LongId NeighborhoodId, int Role) : CodecServerCommand<NeighborhoodRoleChangedServerCommand>, ICommandCodecProvider<NeighborhoodRoleChangedServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<NeighborhoodRoleChangedServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.LongId,
            static command => command.NeighborhoodId,
            Codecs.VarInt,
            static command => command.Role,
            static (neighborhoodId, role) => new NeighborhoodRoleChangedServerCommand(neighborhoodId, role)
        )
    );
}
