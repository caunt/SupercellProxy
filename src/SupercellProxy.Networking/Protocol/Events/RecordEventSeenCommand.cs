using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events;

/// <summary>
/// Serverbound command recording that the player looked at a live event's announcement: a targeted event is remembered per
/// showing, any other event once.
/// </summary>
/// <param name="EventId">Live event that was looked at.</param>
/// <param name="Context0">First context number native sends with the record; not used when recording.</param>
/// <param name="Context1">Second context number native sends with the record; not used when recording.</param>
public sealed record RecordEventSeenCommand([property: JsonPropertyName("EventId")] int EventId, int Context0, int Context1) : CodecCommand<RecordEventSeenCommand>, ICommandCodecProvider<RecordEventSeenCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RecordEventSeenCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.Int32,
            static command => command.EventId,
            Codecs.Int32,
            static command => command.Context0,
            Codecs.Int32,
            static command => command.Context1,
            static (id, context0, context1) => new RecordEventSeenCommand(id, context0, context1)
        )
    );
}
