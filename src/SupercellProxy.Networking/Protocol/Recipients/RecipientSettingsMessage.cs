using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Recipients;

/// <summary>Delivers the recipient settings to the social manager.</summary>
/// <param name="Flag">The flag.</param>
/// <param name="Value">The value.</param>
public sealed record RecipientSettingsMessage(bool Flag, int Value) : CodecMessage<RecipientSettingsMessage>, ICodecProvider<RecipientSettingsMessage>
{
    /// <inheritdoc />
    public static ICodec<RecipientSettingsMessage> Codec { get; } = Codecs.Composite(
        Codecs.Boolean,
        static message => message.Flag,
        Codecs.VarInt,
        static message => message.Value,
        static (flag, @value) => new RecipientSettingsMessage(flag, value)
    );
}
