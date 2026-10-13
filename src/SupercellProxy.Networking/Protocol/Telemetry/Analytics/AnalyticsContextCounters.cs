using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Telemetry.Analytics;

/// <summary>The fourth optional analytics context part: four values.</summary>
/// <param name="Value0">The first value.</param>
/// <param name="Value1">The second value.</param>
/// <param name="Value2">The third value.</param>
/// <param name="Value3">The fourth value.</param>
public sealed record AnalyticsContextCounters(int Value0, int Value1, int Value2, int Value3) : ICodecProvider<AnalyticsContextCounters>
{
    /// <inheritdoc />
    public static ICodec<AnalyticsContextCounters> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static value => value.Value0,
        Codecs.VarInt,
        static value => value.Value1,
        Codecs.VarInt,
        static value => value.Value2,
        Codecs.VarInt,
        static value => value.Value3,
        static (value0, value1, value2, value3) => new AnalyticsContextCounters(value0, value1, value2, value3)
    );
}
