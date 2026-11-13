using System.Runtime.InteropServices;

using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Telemetry.Analytics;

/// <summary>An analytics context part of ten 64-bit values; three of the six context parts use it.</summary>
/// <param name="Values">The ten values.</param>
public sealed record AnalyticsContextStatistics(ReadOnlyMemory<long> Values) : ICodecProvider<AnalyticsContextStatistics>
{
    private const int ValueCount = 10;

    /// <inheritdoc />
    public static ICodec<AnalyticsContextStatistics> Codec { get; } = Codecs.Composite(Codecs.Map(Codecs.FixedArray(Codecs.BigEndianInt64, ValueCount),
            static values => values.AsMemory(),
            static values => ImmutableCollectionsMarshal.AsImmutableArray(values.ToArray())),
        static statistics => statistics.Values,
        static values => new AnalyticsContextStatistics(values));
}
