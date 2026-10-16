using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Telemetry.Soil;

/// <summary>Reports that a Soil SDK interface was used.</summary>
/// <param name="Timezone">Time zone name of the device, such as Europe/Kiev.</param>
/// <param name="DeviceId">The device's 16-digit hexadecimal identifier.</param>
/// <param name="Value0">Number that is 1 in every capture; its meaning is not identified.</param>
/// <param name="Value1">Number that is 0 in every capture; its meaning is not identified.</param>
/// <param name="ContentId">The Soil content the event is about.</param>
/// <param name="TimestampMilliseconds">Unix time in milliseconds when the event happened.</param>
/// <param name="Value2">Number that is 3 in every capture; its meaning is not identified.</param>
/// <param name="Text">Text that is empty in every capture; its meaning is not identified.</param>
/// <param name="Value3">Number that is -1 in every capture; its meaning is not identified.</param>
/// <param name="Properties">Named details of the event; the captures carry the local notification id as <c>notificationGameId</c>.</param>
public sealed record SoilUserInterfaceUsedAnalyticsMessage(
    string? Timezone,
    string? DeviceId,
    int Value0,
    int Value1,
    string? ContentId,
    long TimestampMilliseconds,
    int Value2,
    string? Text,
    int Value3,
    ReadOnlyMemory<SoilProperty> Properties
) : CodecMessage<SoilUserInterfaceUsedAnalyticsMessage>, ICodecProvider<SoilUserInterfaceUsedAnalyticsMessage>
{
    /// <inheritdoc />
    public static ICodec<SoilUserInterfaceUsedAnalyticsMessage> Codec { get; } = Codecs.Composite(
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
        static (timezone, deviceId, value0, value1, contentId, timestampMilliseconds, value2, text, value3, properties) => new SoilUserInterfaceUsedAnalyticsMessage(timezone, deviceId, value0, value1, contentId, timestampMilliseconds, value2, text, value3, properties)
    );
}
