using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Telemetry.Soil;

/// <summary>Reports progress in the Soil SDK introduction tutorial.</summary>
/// <param name="Timezone">The timezone.</param>
/// <param name="DeviceId">The device id.</param>
/// <param name="Value0">The value0.</param>
/// <param name="Value1">The value1.</param>
/// <param name="ContentId">The Soil content the event is about.</param>
/// <param name="TimestampMilliseconds">The timestamp milliseconds.</param>
/// <param name="Value2">The value2.</param>
/// <param name="Text">The text.</param>
/// <param name="Value3">The value3.</param>
/// <param name="Properties">The properties.</param>
/// <param name="Value4">The value4.</param>
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
