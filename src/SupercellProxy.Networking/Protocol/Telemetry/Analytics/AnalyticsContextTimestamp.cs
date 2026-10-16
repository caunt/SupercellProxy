using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Telemetry.Analytics;

/// <summary>The fifth optional analytics context part: a value and a 64-bit time.</summary>
/// <param name="Value">Small number, 1 for launch and 2 for Farm_load in the captures; its meaning is not identified.</param>
/// <param name="Time">Length close to the session length, which looks like milliseconds; its exact meaning is not identified.</param>
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
