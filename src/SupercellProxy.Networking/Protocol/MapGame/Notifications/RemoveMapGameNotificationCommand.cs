using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Notifications;

/// <summary>Removes one pending Valley notification after it has been presented.</summary>
public sealed record RemoveMapGameNotificationCommand([property: System.Text.Json.Serialization.JsonPropertyName("NotificationGlobalId")] int NotificationGlobalId) : CodecCommand<RemoveMapGameNotificationCommand>, ICommandCodecProvider<RemoveMapGameNotificationCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RemoveMapGameNotificationCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.NotificationGlobalId,
            static (notificationGlobalId) => new RemoveMapGameNotificationCommand(notificationGlobalId)
        )
    );
}
