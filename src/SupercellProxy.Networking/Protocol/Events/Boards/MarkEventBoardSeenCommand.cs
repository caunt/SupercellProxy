using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Boards;

/// <summary>
/// Serverbound command recording that the player has seen an event on the event board, which clears its "new" marker.
/// </summary>
/// <param name="EventId">Event board event the player has seen.</param>
public sealed record MarkEventBoardSeenCommand([property: JsonPropertyName("EventId")] int EventId) : CodecCommand<MarkEventBoardSeenCommand>, ICommandCodecProvider<MarkEventBoardSeenCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkEventBoardSeenCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt, static command => command.EventId, static eventId => new MarkEventBoardSeenCommand(eventId)));
}
