using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Tasks;

/// <summary>Collects, pins, discards, speeds up, renews or buys a replacement for one task of an event task list.</summary>
/// <param name="EventId">Task event the task belongs to.</param>
/// <param name="TaskIndex">Slot of the task in the event's task list.</param>
/// <param name="Action">The native task action: 1 collect, 2 select (pin), 3 discard, 4 speed up, 5 renew, 6 buy a replacement.</param>
public sealed record PerformEventTaskActionCommand(int EventId, int TaskIndex, int Action) : CodecCommand<PerformEventTaskActionCommand>, ICommandCodecProvider<PerformEventTaskActionCommand>
{
    /// <summary>Collects a finished task's points and replaces the task.</summary>
    public const int CollectAction = 1;

    /// <summary>Discards a task and replaces it.</summary>
    public const int DiscardAction = 3;

    /// <summary>Buys a replacement for a task.</summary>
    public const int PurchaseReplacementAction = 6;

    /// <summary>Renews a task whose timer ran out.</summary>
    public const int RenewAction = 5;

    /// <summary>Pins a task, or starts its timer in events where only the pinned task makes progress.</summary>
    public const int SelectAction = 2;

    /// <summary>Finishes a pinned task's timer with diamonds.</summary>
    public const int SpeedUpAction = 4;

    /// <inheritdoc />
    public static CommandCodec<PerformEventTaskActionCommand> Codec { get; } = Codecs.Command(
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
