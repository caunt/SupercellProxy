using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Tasks;

/// <summary>Updates an event task.</summary>
/// <param name="EventId">The event id.</param>
/// <param name="TaskIndex">The task index.</param>
/// <param name="Progress">The progress.</param>
/// <param name="Completed">The completed.</param>
/// <param name="TaskId">The task id.</param>
/// <param name="Points">The points.</param>
public sealed record EventTaskUpdateServerCommand(int EventId, int TaskIndex, int Progress, bool Completed, long TaskId, int Points) : CodecServerCommand<EventTaskUpdateServerCommand>, ICommandCodecProvider<EventTaskUpdateServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<EventTaskUpdateServerCommand> Codec { get; } = Codecs.Command(
        type: 366,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.EventId,
            Codecs.VarInt,
            static command => command.TaskIndex,
            Codecs.VarInt,
            static command => command.Progress,
            Codecs.Boolean,
            static command => command.Completed,
            Codecs.VarLong,
            static command => command.TaskId,
            Codecs.VarInt,
            static command => command.Points,
            static (eventId, taskIndex, progress, completed, taskId, points) => new EventTaskUpdateServerCommand(eventId, taskIndex, progress, completed, taskId, points)
        )
    );
}
