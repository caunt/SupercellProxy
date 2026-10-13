using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Tasks;

/// <summary>Submits the goods of an event task.</summary>
/// <param name="EventId">The event id.</param>
/// <param name="TaskIndex">The task index.</param>
/// <param name="UseDiamonds">The use diamonds.</param>
/// <param name="GoodIds">The good ids.</param>
/// <param name="TaskId">The task id.</param>
/// <param name="Points">The points.</param>
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
