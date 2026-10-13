using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Telemetry.Analytics;

/// <summary>The fifth optional analytics context part: a value and a 64-bit time.</summary>
/// <param name="Value">The value.</param>
/// <param name="Time">The time.</param>
public sealed record AnalyticsContextTimestamp(int Value, long Time) : ICodecProvider<AnalyticsContextTimestamp>
{
    /// <inheritdoc />
    public static ICodec<AnalyticsContextTimestamp> Codec { get; } = Codecs.Composite(
        Codecs.Int32,
        static value => value.Value,
        Codecs.Int64,
        static value => value.Time,
        static (@value, time) => new AnalyticsContextTimestamp(value, time)
    );
}
