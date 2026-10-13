using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Derby;

/// <summary>Updates the progress of a neighbor's derby task.</summary>
/// <param name="NeighborhoodId">The neighborhood id.</param>
/// <param name="MemberHomeId">The member home id.</param>
/// <param name="TaskId">The task id.</param>
/// <param name="Points">The points.</param>
/// <param name="State">The state.</param>
public sealed record DerbyTaskProgressServerCommand(LongId NeighborhoodId, LongId MemberHomeId, int TaskId, int Points, int State) : CodecServerCommand<DerbyTaskProgressServerCommand>, ICommandCodecProvider<DerbyTaskProgressServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<DerbyTaskProgressServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.LongId,
            static command => command.NeighborhoodId,
            Codecs.LongId,
            static command => command.MemberHomeId,
            Codecs.Int32,
            static command => command.TaskId,
            Codecs.Int32,
            static command => command.Points,
            Codecs.Int32,
            static command => command.State,
            static (neighborhoodId, memberHomeId, taskId, points, state) => new DerbyTaskProgressServerCommand(neighborhoodId, memberHomeId, taskId, points, state)
        )
    );
}
