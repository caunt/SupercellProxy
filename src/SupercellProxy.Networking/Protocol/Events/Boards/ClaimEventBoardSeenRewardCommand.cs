using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Boards;

/// <summary>
/// Serverbound command claiming the coin reward an event board event gives the first time the player looks at it.
/// </summary>
/// <param name="EventId">Event board event whose first-look reward to claim.</param>
public sealed record ClaimEventBoardSeenRewardCommand([property: JsonPropertyName("EventId")] int EventId) : CodecCommand<ClaimEventBoardSeenRewardCommand>, ICommandCodecProvider<ClaimEventBoardSeenRewardCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimEventBoardSeenRewardCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.EventId, static eventId => new ClaimEventBoardSeenRewardCommand(eventId)),
        CommandLayout.MetadataFirst
    );
}
