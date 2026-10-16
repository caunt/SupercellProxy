using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Tasks;

/// <summary>
/// Serverbound command recording that the player has seen a task in an event's task list, which clears its "new" marker.
/// </summary>
/// <param name="TaskIndex">Slot of the task in the event's task list.</param>
/// <param name="EventId">Event board event the task list belongs to.</param>
public sealed record MarkEventTasksSeenCommand(int TaskIndex, [property: JsonPropertyName("EventId")] int EventId) : CodecCommand<MarkEventTasksSeenCommand>, ICommandCodecProvider<MarkEventTasksSeenCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkEventTasksSeenCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.TaskIndex,
            Codecs.VarInt,
            static command => command.EventId,
            static (taskIndex, eventId) => new MarkEventTasksSeenCommand(taskIndex, eventId)
        ),
        CommandLayout.MetadataFirst
    );
}
