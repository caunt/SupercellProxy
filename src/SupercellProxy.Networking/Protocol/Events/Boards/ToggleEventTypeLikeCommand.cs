using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Boards;

/// <summary>Toggles the liked state shared by events of the selected event's type.</summary>
/// <param name="EventId">The live event id of an event on the event board, written as a VarInt; it names the event type whose like flag is toggled.</param>
public sealed record ToggleEventTypeLikeCommand(int EventId) : CodecCommand<ToggleEventTypeLikeCommand>, ICommandCodecProvider<ToggleEventTypeLikeCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ToggleEventTypeLikeCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt, static command => command.EventId, static eventId => new ToggleEventTypeLikeCommand(eventId)),
        CommandLayout.MetadataFirst);
}
