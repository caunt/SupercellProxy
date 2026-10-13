using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Tasks;

/// <summary>
/// Defines the Mark Task Event Opened Command contract.
/// </summary>
/// <summary>
/// Defines the Event Id contract.
/// </summary>
public sealed record MarkTaskEventOpenedCommand([property: System.Text.Json.Serialization.JsonPropertyName("EventId")] int EventId) : CodecCommand<MarkTaskEventOpenedCommand>, ICommandCodecProvider<MarkTaskEventOpenedCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkTaskEventOpenedCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt, static command => command.EventId, static (eventId) => new MarkTaskEventOpenedCommand(eventId)));
}
