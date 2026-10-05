using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.MapGame.Notifications;

/// <summary>Removes one pending Valley notification after it has been presented.</summary>
public sealed record RemoveMapGameNotificationCommand([property: System.Text.Json.Serialization.JsonPropertyName("NotificationGlobalId")] int NotificationGlobalId) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.RemoveMapGameNotificationCommandType;

    /// <summary>Decodes a command from its native wire representation.</summary>
    public static RemoveMapGameNotificationCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int notificationGlobalId = stream.ReadVarInt();

        return new RemoveMapGameNotificationCommand(notificationGlobalId);
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVarInt(NotificationGlobalId);
    }
}
