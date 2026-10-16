using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>Serverbound command closing a Farm Pass notification so it is not shown again.</summary>
/// <param name="NotificationGlobalId">Global id of the notification's table entry.</param>
public sealed record DismissFarmPassNotificationCommand([property: JsonPropertyName("NotificationGlobalId")] int NotificationGlobalId) : CodecCommand<DismissFarmPassNotificationCommand>, ICommandCodecProvider<DismissFarmPassNotificationCommand>
{
    /// <inheritdoc />
    public static CommandCodec<DismissFarmPassNotificationCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.NotificationGlobalId,
            static notification => new DismissFarmPassNotificationCommand(notification)
        )
    );
}
