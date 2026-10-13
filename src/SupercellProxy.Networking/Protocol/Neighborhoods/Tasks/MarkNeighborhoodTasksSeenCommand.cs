using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Tasks;

/// <summary>Marks the neighborhood tasks as seen.</summary>
public sealed record MarkNeighborhoodTasksSeenCommand : CodecCommand<MarkNeighborhoodTasksSeenCommand>, ICommandCodecProvider<MarkNeighborhoodTasksSeenCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkNeighborhoodTasksSeenCommand> Codec { get; } = Codecs.Command(type: 596, Codecs.Unit(static () => new MarkNeighborhoodTasksSeenCommand()));
}
