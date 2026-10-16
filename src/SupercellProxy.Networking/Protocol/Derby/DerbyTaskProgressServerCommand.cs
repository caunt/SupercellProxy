using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Derby;

/// <summary>Updates the progress of a neighbor's derby task.</summary>
/// <param name="NeighborhoodId">Neighbourhood whose derby the task belongs to.</param>
/// <param name="MemberHomeId">Farm of the neighbour working on the task.</param>
/// <param name="TaskId">Derby task being worked on.</param>
/// <param name="Points">Derby points the task is worth.</param>
/// <param name="State">New state of the task, as native numbers derby task states.</param>
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
