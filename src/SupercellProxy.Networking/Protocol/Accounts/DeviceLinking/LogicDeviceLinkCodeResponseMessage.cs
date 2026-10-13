using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts.DeviceLinking;

/// <summary>Delivers a device link code; the client only records the message.</summary>
/// <param name="Value0">The value0.</param>
/// <param name="Code">The code.</param>
/// <param name="Value1">The value1.</param>
/// <param name="Value2">The value2.</param>
/// <param name="Value3">The value3.</param>
public sealed record LogicDeviceLinkCodeResponseMessage(int Value0, string? Code, int Value1, int Value2, int Value3) : CodecMessage<LogicDeviceLinkCodeResponseMessage>, ICodecProvider<LogicDeviceLinkCodeResponseMessage>
{
    /// <inheritdoc />
    public static ICodec<LogicDeviceLinkCodeResponseMessage> Codec { get; } = Codecs.Composite(
        Codecs.Int32,
        static message => message.Value0,
        Codecs.OptionalString,
        static message => message.Code,
        Codecs.Int32,
        static message => message.Value1,
        Codecs.Int32,
        static message => message.Value2,
        Codecs.Int32,
        static message => message.Value3,
        static (value0, code, value1, value2, value3) => new LogicDeviceLinkCodeResponseMessage(value0, code, value1, value2, value3)
    );
}
