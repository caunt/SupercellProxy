using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Telemetry;

/// <summary>Reports an audio analytics event such as an interrupted music track.</summary>
/// <param name="Timestamp">The timestamp.</param>
/// <param name="Value0">The value0.</param>
/// <param name="Value1">The value1.</param>
/// <param name="Value2">The value2.</param>
/// <param name="Flag0">The flag0.</param>
/// <param name="Flag1">The flag1.</param>
/// <param name="Values0">The values0.</param>
/// <param name="Value3">The value3.</param>
/// <param name="Value4">The value4.</param>
/// <param name="Value5">The value5.</param>
/// <param name="Value6">The value6.</param>
/// <param name="DeviceId">The device id.</param>
/// <param name="Texts">The texts.</param>
/// <param name="Value7">The value7.</param>
/// <param name="Values1">The values1.</param>
/// <param name="Value8">The value8.</param>
/// <param name="Value9">The value9.</param>
/// <param name="Flag2">The flag2.</param>
/// <param name="EventName">The event name.</param>
/// <param name="Properties">The properties.</param>
public sealed record AudioAnalyticsMessage(
    long Timestamp,
    int Value0,
    int Value1,
    float Value2,
    bool Flag0,
    bool Flag1,
    ReadOnlyMemory<int> Values0,
    float Value3,
    float Value4,
    float Value5,
    float Value6,
    string? DeviceId,
    ReadOnlyMemory<string?> Texts,
    int Value7,
    ReadOnlyMemory<int> Values1,
    int Value8,
    int Value9,
    bool Flag2,
    string? EventName,
    ReadOnlyMemory<AudioAnalyticsProperty> Properties
) : CodecMessage<AudioAnalyticsMessage>, ICodecProvider<AudioAnalyticsMessage>
{
    /// <inheritdoc />
    public static ICodec<AudioAnalyticsMessage> Codec { get; } = Codecs.Composite(
        Codecs.Int64,
        static message => message.Timestamp,
        Codecs.Int32,
        static message => message.Value0,
        Codecs.Int32,
        static message => message.Value1,
        Codecs.Float,
        static message => message.Value2,
        Codecs.Boolean,
        static message => message.Flag0,
        Codecs.Boolean,
        static message => message.Flag1,
        Codecs.Memory(Codecs.Int32),
        static message => message.Values0,
        FloatValues.Codec,
        static message => new FloatValues(message.Value3, message.Value4, message.Value5, message.Value6),
        Codecs.OptionalString,
        static message => message.DeviceId,
        Codecs.Memory(Codecs.OptionalString),
        static message => message.Texts,
        Codecs.Int32,
        static message => message.Value7,
        Codecs.Memory(Codecs.Int32),
        static message => message.Values1,
        TailValues.Codec,
        static message => new TailValues(message.Value8, message.Value9, message.Flag2),
        Codecs.OptionalString,
        static message => message.EventName,
        Codecs.Memory(AudioAnalyticsProperty.Codec),
        static message => message.Properties,
        static (
            timestamp,
            value0,
            value1,
            value2,
            flag0,
            flag1,
            values0,
            floatValues,
            deviceId,
            texts,
            value7,
            values1,
            tailValues,
            eventName,
            properties
        ) => new AudioAnalyticsMessage(
            timestamp,
            value0,
            value1,
            value2,
            flag0,
            flag1,
            values0,
            floatValues.Value3,
            floatValues.Value4,
            floatValues.Value5,
            floatValues.Value6,
            deviceId,
            texts,
            value7,
            values1,
            tailValues.Value8,
            tailValues.Value9,
            tailValues.Flag2,
            eventName,
            properties
        )
    );

    // The four float values native reads after the first list.
    private readonly record struct FloatValues(float Value3, float Value4, float Value5, float Value6)
    {
        public static ICodec<FloatValues> Codec { get; } = Codecs.Composite(
            Codecs.Float,
            static segment => segment.Value3,
            Codecs.Float,
            static segment => segment.Value4,
            Codecs.Float,
            static segment => segment.Value5,
            Codecs.Float,
            static segment => segment.Value6,
            static (value3, value4, value5, value6) => new FloatValues(value3, value4, value5, value6)
        );
    }

    // The values native reads after the second list.
    private readonly record struct TailValues(int Value8, int Value9, bool Flag2)
    {
        public static ICodec<TailValues> Codec { get; } = Codecs.Composite(
            Codecs.Int32,
            static segment => segment.Value8,
            Codecs.Int32,
            static segment => segment.Value9,
            Codecs.Boolean,
            static segment => segment.Flag2,
            static (value8, value9, flag2) => new TailValues(value8, value9, flag2)
        );
    }
}
