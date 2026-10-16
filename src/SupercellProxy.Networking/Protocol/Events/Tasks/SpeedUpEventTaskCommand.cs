using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Tasks;

/// <summary>Pays diamonds to finish an event task timer.</summary>
/// <param name="EventId">Task event the task belongs to.</param>
/// <param name="TaskIndex">Slot of the waiting task in the event's task list.</param>
public sealed record SpeedUpEventTaskCommand(int EventId, int TaskIndex) : CodecCommand<SpeedUpEventTaskCommand>, ICommandCodecProvider<SpeedUpEventTaskCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SpeedUpEventTaskCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.EventId,
            Codecs.VarInt,
            static command => command.TaskIndex,
            static (eventId, taskIndex) => new SpeedUpEventTaskCommand(eventId, taskIndex)
        )
    );
}
