using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Boards;

/// <summary>
/// Defines the Mark Event Board Seen Command contract.
/// </summary>
/// <summary>
/// Defines the Event Id contract.
/// </summary>
public sealed record MarkEventBoardSeenCommand([property: System.Text.Json.Serialization.JsonPropertyName("EventId")] int EventId) : CodecCommand<MarkEventBoardSeenCommand>, ICommandCodecProvider<MarkEventBoardSeenCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkEventBoardSeenCommand> Codec { get; } = Codecs.Command(
        type: 116,
        Codecs.Composite(Codecs.VarInt, static command => command.EventId, static (eventId) => new MarkEventBoardSeenCommand(eventId))
    );
}
