using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


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
public sealed record ClearEventLeaderboardNotificationCommand([property: System.Text.Json.Serialization.JsonPropertyName("EventId")] int EventId, int ButtonState) : Command
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.ClearEventLeaderboardNotificationCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static ClearEventLeaderboardNotificationCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int eventId = stream.ReadVarInt();
        int state = stream.ReadVarInt();

        return new ClearEventLeaderboardNotificationCommand(eventId, state);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(EventId);
        stream.WriteVarInt(ButtonState);
    }
}
