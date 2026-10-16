using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Tasks;

/// <summary>
/// Serverbound command recording that the player opened a task event for the first time.
/// </summary>
/// <param name="EventId">Event board event that was opened.</param>
public sealed record MarkTaskEventOpenedCommand([property: JsonPropertyName("EventId")] int EventId) : CodecCommand<MarkTaskEventOpenedCommand>, ICommandCodecProvider<MarkTaskEventOpenedCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkTaskEventOpenedCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt, static command => command.EventId, static eventId => new MarkTaskEventOpenedCommand(eventId)));
}
