using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Tasks;

/// <summary>
/// Serverbound command clearing the notification badge on an event's leaderboard button after the player looked at it.
/// </summary>
/// <param name="EventId">Event board event whose leaderboard button is cleared.</param>
/// <param name="ButtonState">Display state the button changes to, as native numbers it.</param>
public sealed record ClearEventLeaderboardNotificationCommand([property: JsonPropertyName("EventId")] int EventId, int ButtonState) : CodecCommand<ClearEventLeaderboardNotificationCommand>, ICommandCodecProvider<ClearEventLeaderboardNotificationCommand>
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
