using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Tasks;

/// <summary>Renews, selects, discards, speeds up or pins one task of an event task list.</summary>
/// <param name="EventId">The event id.</param>
/// <param name="TaskIndex">The task index.</param>
/// <param name="Action">The native task action: 1 renew, 2 select, 3 discard, 4 speed up, 5 pin.</param>
public sealed record PerformEventTaskActionCommand(int EventId, int TaskIndex, int Action) : CodecCommand<PerformEventTaskActionCommand>, ICommandCodecProvider<PerformEventTaskActionCommand>
{
    /// <inheritdoc />
    public static CommandCodec<PerformEventTaskActionCommand> Codec { get; } = Codecs.Command(
        type: 359,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.EventId,
            Codecs.VarInt,
            static command => command.TaskIndex,
            Codecs.VarInt,
            static command => command.Action,
            static (eventId, taskIndex, action) => new PerformEventTaskActionCommand(eventId, taskIndex, action)
        )
    );
}
