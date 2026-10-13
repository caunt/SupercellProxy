using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts.DeviceLinking;

/// <summary>Carries a device link value and flag.</summary>
/// <param name="Value">The value.</param>
/// <param name="Flag">The flag.</param>
public sealed record DeviceLinkStateMessage(int Value, bool Flag) : CodecMessage<DeviceLinkStateMessage>, ICodecProvider<DeviceLinkStateMessage>
{
    /// <inheritdoc />
    public static ICodec<DeviceLinkStateMessage> Codec { get; } = Codecs.Composite(
        Codecs.Int32,
        static message => message.Value,
        Codecs.Boolean,
        static message => message.Flag,
        static (@value, flag) => new DeviceLinkStateMessage(value, flag)
    );
}
