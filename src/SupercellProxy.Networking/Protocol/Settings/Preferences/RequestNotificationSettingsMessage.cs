using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Settings.Preferences;

/// <summary>Requests the notification settings.</summary>
public sealed record RequestNotificationSettingsMessage : CodecMessage<RequestNotificationSettingsMessage>, ICodecProvider<RequestNotificationSettingsMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestNotificationSettingsMessage> Codec { get; } = Codecs.Unit(static () => new RequestNotificationSettingsMessage());
}
