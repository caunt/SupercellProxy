using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts.DeviceLinking;

/// <summary>Delivers a device link code; the client only records the message.</summary>
/// <param name="Value0">Number sent before the code; not identified, since the client never reads the message's fields.</param>
/// <param name="Code">Link code to enter on the other device.</param>
/// <param name="Value1">First number after the code; not identified.</param>
/// <param name="Value2">Second number after the code; not identified.</param>
/// <param name="Value3">Third number after the code; not identified.</param>
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
