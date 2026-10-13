using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Tasks;

/// <summary>Pays diamonds to finish an event task timer.</summary>
/// <param name="EventId">The event id.</param>
/// <param name="TaskIndex">The task index.</param>
public sealed record SpeedUpEventTaskCommand(int EventId, int TaskIndex) : CodecCommand<SpeedUpEventTaskCommand>, ICommandCodecProvider<SpeedUpEventTaskCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SpeedUpEventTaskCommand> Codec { get; } = Codecs.Command(
        type: 363,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.EventId,
            Codecs.VarInt,
            static command => command.TaskIndex,
            static (eventId, taskIndex) => new SpeedUpEventTaskCommand(eventId, taskIndex)
        )
    );
}
