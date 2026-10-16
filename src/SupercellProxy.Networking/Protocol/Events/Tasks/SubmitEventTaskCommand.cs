using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Tasks;

/// <summary>Submits the goods of an event task.</summary>
/// <param name="EventId">Task event the task belongs to.</param>
/// <param name="TaskIndex">Slot of the task in the event's task list.</param>
/// <param name="UseDiamonds">Whether diamonds pay for goods the player is missing.</param>
/// <param name="GoodIds">The goods handed in for the task.</param>
/// <param name="TaskId">Server identifier of the task in that slot.</param>
/// <param name="Points">Event points the client expects the task to give.</param>
public sealed record SubmitEventTaskCommand(int EventId, int TaskIndex, bool UseDiamonds, ReadOnlyMemory<long> GoodIds, long TaskId, int Points) : CodecCommand<SubmitEventTaskCommand>, ICommandCodecProvider<SubmitEventTaskCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SubmitEventTaskCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.EventId,
            Codecs.VarInt,
            static command => command.TaskIndex,
            Codecs.Boolean,
            static command => command.UseDiamonds,
            Codecs.Memory(Codecs.VarLong),
            static command => command.GoodIds,
            Codecs.VarLong,
            static command => command.TaskId,
            Codecs.VarInt,
            static command => command.Points,
            static (eventId, taskIndex, useDiamonds, goodIds, taskId, points) => new SubmitEventTaskCommand(eventId, taskIndex, useDiamonds, goodIds, taskId, points)
        )
    );
}
