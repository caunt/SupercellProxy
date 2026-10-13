using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Events;

/// <summary>Records the last seen Valley event.</summary>
/// <param name="EventId">The seen event id.</param>
public sealed record MarkMapGameEventSeenCommand(int EventId) : CodecCommand<MarkMapGameEventSeenCommand>, ICommandCodecProvider<MarkMapGameEventSeenCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkMapGameEventSeenCommand> Codec { get; } = Codecs.Command(
        type: 297,
        Codecs.Composite(Codecs.VarInt, static command => command.EventId, static (eventId) => new MarkMapGameEventSeenCommand(eventId))
    );
}
