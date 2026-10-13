using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>
/// Defines the Dismiss Farm Pass Notification Command contract.
/// </summary>
/// <summary>
/// Defines the Notification Global Id contract.
/// </summary>
public sealed record DismissFarmPassNotificationCommand([property: System.Text.Json.Serialization.JsonPropertyName("NotificationGlobalId")] int NotificationGlobalId) : CodecCommand<DismissFarmPassNotificationCommand>, ICommandCodecProvider<DismissFarmPassNotificationCommand>
{
    /// <inheritdoc />
    public static CommandCodec<DismissFarmPassNotificationCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.NotificationGlobalId,
            static (notification) => new DismissFarmPassNotificationCommand(notification)
        )
    );
}
