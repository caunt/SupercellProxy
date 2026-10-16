using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Telemetry.Soil;

/// <summary>Reports progress in the Soil SDK introduction tutorial.</summary>
/// <param name="Timezone">Time zone name of the device, such as Europe/Kiev.</param>
/// <param name="DeviceId">The device's 16-digit hexadecimal identifier.</param>
/// <param name="Value0">First number of the event; not seen in the captures, and its meaning is not identified.</param>
/// <param name="Value1">Second number of the event; not seen in the captures, and its meaning is not identified.</param>
/// <param name="ContentId">The Soil content the event is about.</param>
/// <param name="TimestampMilliseconds">Unix time in milliseconds when the event happened.</param>
/// <param name="Value2">Third number of the event; not seen in the captures, and its meaning is not identified.</param>
/// <param name="Text">Text sent with the event; its meaning is not identified.</param>
/// <param name="Value3">Fourth number of the event; not seen in the captures, and its meaning is not identified.</param>
/// <param name="Properties">Named details of the event.</param>
/// <param name="Value4">Fifth number of the event; not seen in the captures, and its meaning is not identified.</param>
public sealed record SoilIntroTutorialAnalyticsMessage(
    string? Timezone,
    string? DeviceId,
    int Value0,
    int Value1,
    string? ContentId,
    long TimestampMilliseconds,
    int Value2,
    string? Text,
    int Value3,
    ReadOnlyMemory<SoilProperty> Properties,
    int Value4
) : CodecMessage<SoilIntroTutorialAnalyticsMessage>, ICodecProvider<SoilIntroTutorialAnalyticsMessage>
{
    /// <inheritdoc />
    public static ICodec<SoilIntroTutorialAnalyticsMessage> Codec { get; } = Codecs.Composite(
        Codecs.OptionalString,
        static message => message.Timezone,
        Codecs.OptionalString,
        static message => message.DeviceId,
        Codecs.VarInt,
        static message => message.Value0,
        Codecs.VarInt,
        static message => message.Value1,
        Codecs.OptionalString,
        static message => message.ContentId,
        Codecs.VarLong,
        static message => message.TimestampMilliseconds,
        Codecs.VarInt,
        static message => message.Value2,
        Codecs.OptionalString,
        static message => message.Text,
        Codecs.VarInt,
        static message => message.Value3,
        Codecs.Memory(SoilProperty.Codec),
        static message => message.Properties,
        Codecs.VarInt,
        static message => message.Value4,
        static (timezone, deviceId, value0, value1, contentId, timestampMilliseconds, value2, text, value3, properties, value4) => new SoilIntroTutorialAnalyticsMessage(timezone, deviceId, value0, value1, contentId, timestampMilliseconds, value2, text, value3, properties, value4)
    );
}
