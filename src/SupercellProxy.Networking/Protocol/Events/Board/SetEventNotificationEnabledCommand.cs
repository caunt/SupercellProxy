using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Board;

/// <summary>Toggles notifications for an event board entry.</summary>
/// <param name="EventId">The event id.</param>
public sealed record SetEventNotificationEnabledCommand(int EventId) : CodecCommand<SetEventNotificationEnabledCommand>, ICommandCodecProvider<SetEventNotificationEnabledCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SetEventNotificationEnabledCommand> Codec { get; } = Codecs.Command(
        type: 618,
        Codecs.Composite(Codecs.VarInt, static command => command.EventId, static (eventId) => new SetEventNotificationEnabledCommand(eventId)),
        CommandLayout.MetadataFirst
    );
}
