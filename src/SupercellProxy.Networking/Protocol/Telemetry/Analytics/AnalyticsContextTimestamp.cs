using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Telemetry.Analytics;

/// <summary>The fifth optional analytics context part: a timestamp collector subtype and a 64-bit duration.</summary>
/// <param name="Value">
/// Release-specific timestamp collector subtype. This is not the context section's collector kind.
/// </param>
/// <param name="Time">Active session duration in milliseconds, excluding paused intervals.</param>
/// <remarks>The complete subtype mapping is release-specific and has not been recovered.</remarks>
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
