using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town.Hall;

/// <summary>Records the town calendar event the player has seen, as the native town calendar does when it opens.</summary>
/// <param name="CalendarEventGlobalId">Gets the calendar event stored as the last seen personal neighborhood event.</param>
public sealed record MarkTownCalendarEventSeenCommand(int CalendarEventGlobalId) : CodecCommand<MarkTownCalendarEventSeenCommand>, ICommandCodecProvider<MarkTownCalendarEventSeenCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkTownCalendarEventSeenCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.CalendarEventGlobalId,
            static (calendarEventGlobalId) => new MarkTownCalendarEventSeenCommand(calendarEventGlobalId)
        )
    );
}
