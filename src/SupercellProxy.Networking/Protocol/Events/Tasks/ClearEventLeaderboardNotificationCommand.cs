using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Tasks;

/// <summary>
/// Defines the Clear Event Leaderboard Notification Command contract.
/// </summary>
/// <summary>
/// Defines the Event Id contract.
/// </summary>
/// <summary>
/// Defines the Button State contract.
/// </summary>
public sealed record ClearEventLeaderboardNotificationCommand([property: System.Text.Json.Serialization.JsonPropertyName("EventId")] int EventId, int ButtonState) : CodecCommand<ClearEventLeaderboardNotificationCommand>, ICommandCodecProvider<ClearEventLeaderboardNotificationCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClearEventLeaderboardNotificationCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.EventId,
            Codecs.VarInt,
            static command => command.ButtonState,
            static (eventId, state) => new ClearEventLeaderboardNotificationCommand(eventId, state)
        )
    );
}
