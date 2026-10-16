using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts.Settings;

/// <summary>Changes an account setting.</summary>
/// <param name="Setting">Name of the account setting to change.</param>
/// <param name="Value">New value for that setting.</param>
/// <param name="Flag">A flag native reads only when bytes remain.</param>
/// <param name="Data">Data native reads only when bytes remain.</param>
public sealed record ChangeAccountSettingsMessage(string? Setting, int Value, bool? Flag, ReadOnlyMemory<byte>? Data)
    : CodecMessage<ChangeAccountSettingsMessage>, ICodecProvider<ChangeAccountSettingsMessage>
{
    /// <inheritdoc />
    public static ICodec<ChangeAccountSettingsMessage> Codec { get; } = Codecs.Composite(
        Codecs.OptionalString,
        static message => message.Setting,
        Codecs.Int32,
        static message => message.Value,
        Codecs.TrailingValue(Codecs.Boolean),
        static message => message.Flag,
        Codecs.Trailing(Codecs.OptionalByteArray, absent: null, static data => data is not null),
        static message => message.Data,
        static (setting, value, flag, data) => new ChangeAccountSettingsMessage(setting, value, flag, data)
    );
}
