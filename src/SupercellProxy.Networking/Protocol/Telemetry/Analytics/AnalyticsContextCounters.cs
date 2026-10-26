using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Telemetry.Analytics;

/// <summary>The fourth optional analytics context part: four thermal status values.</summary>
/// <param name="Value0">Device thermal status at session start.</param>
/// <param name="Value1">Device thermal status at session end.</param>
/// <param name="Value2">Minimum thermal status observed during the session.</param>
/// <param name="Value3">Maximum thermal status observed during the session.</param>
/// <remarks>
/// Status values are 0 for not set, 1 nominal, 2 warning, 3 serious and 4 critical.
/// </remarks>
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
