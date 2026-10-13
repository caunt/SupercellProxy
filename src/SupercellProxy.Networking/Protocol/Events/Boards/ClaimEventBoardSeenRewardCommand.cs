using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Boards;

/// <summary>
/// Defines the Claim Event Board Seen Reward Command contract.
/// </summary>
/// <summary>
/// Defines the Event Id contract.
/// </summary>
public sealed record ClaimEventBoardSeenRewardCommand([property: System.Text.Json.Serialization.JsonPropertyName("EventId")] int EventId) : CodecCommand<ClaimEventBoardSeenRewardCommand>, ICommandCodecProvider<ClaimEventBoardSeenRewardCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimEventBoardSeenRewardCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.EventId, static (eventId) => new ClaimEventBoardSeenRewardCommand(eventId)),
        CommandLayout.MetadataFirst
    );
}
