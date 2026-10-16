using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Settings.Preferences;

/// <summary>Stores the push notification settings.</summary>
/// <param name="Enabled">Whether the player wants push notifications at all.</param>
/// <param name="Sound">Whether notifications should play a sound.</param>
public sealed record SetNotificationSettingsMessage(bool Enabled, bool Sound) : CodecMessage<SetNotificationSettingsMessage>, ICodecProvider<SetNotificationSettingsMessage>
{
    /// <inheritdoc />
    public static ICodec<SetNotificationSettingsMessage> Codec { get; } = Codecs.Composite(
        Codecs.Boolean,
        static message => message.Enabled,
        Codecs.Boolean,
        static message => message.Sound,
        static (enabled, sound) => new SetNotificationSettingsMessage(enabled, sound)
    );
}
