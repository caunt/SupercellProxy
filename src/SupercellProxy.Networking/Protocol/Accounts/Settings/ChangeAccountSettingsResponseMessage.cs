using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts.Settings;

/// <summary>Answers an account settings change; the client applies the setting value.</summary>
/// <param name="Setting">The setting value; positive values are shifted by five before the client applies them.</param>
/// <param name="Timestamp">64-bit time value sent with the applied setting; its unit and use are not identified.</param>
public sealed record ChangeAccountSettingsResponseMessage(int Setting, long Timestamp) : CodecMessage<ChangeAccountSettingsResponseMessage>, ICodecProvider<ChangeAccountSettingsResponseMessage>
{
    /// <inheritdoc />
    public static ICodec<ChangeAccountSettingsResponseMessage> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static message => message.Setting,
        Codecs.Int64,
        static message => message.Timestamp,
        static (setting, timestamp) => new ChangeAccountSettingsResponseMessage(setting, timestamp)
    );
}
