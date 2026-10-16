using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration.Designs;

/// <summary>Records that a decoration event was shown.</summary>
/// <param name="EventId">Decoration event whose announcement the player has now seen.</param>
public sealed record MarkDecorationEventSeenCommand(int EventId) : CodecCommand<MarkDecorationEventSeenCommand>, ICommandCodecProvider<MarkDecorationEventSeenCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkDecorationEventSeenCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.EventId, static eventId => new MarkDecorationEventSeenCommand(eventId)),
        CommandLayout.MetadataFirst
    );
}
