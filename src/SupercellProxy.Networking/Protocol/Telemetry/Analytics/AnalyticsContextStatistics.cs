using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Telemetry.Analytics;

/// <summary>An ordered analytics statistics payload of ten 64-bit values; three of the six context parts use it.</summary>
/// <param name="Values">
/// During-session minimum and maximum, session-start and session-end samples, then the 1st, 5th, 50th, 95th and 99th percentiles,
/// followed by the sample count.
/// </param>
/// <remarks>
/// The business meaning of the sampled metric is different for each collector and is not established. Only the payload order is common
/// to the configured Statistics collectors.
/// </remarks>
public sealed record AnalyticsContextStatistics(ReadOnlyMemory<long> Values) : ICodecProvider<AnalyticsContextStatistics>
{
    private const int ValueCount = 10;

    /// <inheritdoc />
    public static ICodec<AnalyticsContextStatistics> Codec { get; } = Codecs.Composite(
        Codecs.Map(Codecs.FixedArray(Codecs.Int64, ValueCount), static values => (ReadOnlyMemory<long>)values, static values => values.ToArray()),
        static statistics => statistics.Values,
        static values => new AnalyticsContextStatistics(values)
    );
}
