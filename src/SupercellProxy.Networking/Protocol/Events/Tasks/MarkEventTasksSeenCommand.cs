using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Tasks;

/// <summary>
/// Defines the Mark Event Tasks Seen Command contract.
/// </summary>
/// <summary>
/// Defines the Task Index contract.
/// </summary>
/// <summary>
/// Defines the Event Id contract.
/// </summary>
public sealed record MarkEventTasksSeenCommand(int TaskIndex, [property: System.Text.Json.Serialization.JsonPropertyName("EventId")] int EventId) : CodecCommand<MarkEventTasksSeenCommand>, ICommandCodecProvider<MarkEventTasksSeenCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkEventTasksSeenCommand> Codec { get; } = Codecs.Command(
        type: 549,
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
