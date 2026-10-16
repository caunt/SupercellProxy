using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Tasks;

/// <summary>Updates an event task.</summary>
/// <param name="EventId">Task event the task belongs to.</param>
/// <param name="TaskIndex">Slot of the task in the event's task list.</param>
/// <param name="Progress">Progress count the task has reached.</param>
/// <param name="Completed">Whether the task is now finished.</param>
/// <param name="TaskId">Server identifier of the task in that slot.</param>
/// <param name="Points">Event points the task is worth.</param>
public sealed record EventTaskUpdateServerCommand(int EventId, int TaskIndex, int Progress, bool Completed, long TaskId, int Points) : CodecServerCommand<EventTaskUpdateServerCommand>, ICommandCodecProvider<EventTaskUpdateServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<EventTaskUpdateServerCommand> Codec { get; } = Codecs.Command(
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
