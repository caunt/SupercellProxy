using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Telemetry.Analytics;

/// <summary>The first optional analytics context part: two values and a flag.</summary>
/// <param name="Value0">First number, 92 in the Farm_load capture; its meaning is not identified.</param>
/// <param name="Value1">Second number, equal to the first in the captures; its meaning is not identified.</param>
/// <param name="Flag">Flag that is clear in the captures; its meaning is not identified.</param>
public sealed record AnalyticsContextValues(int Value0, int Value1, bool Flag) : ICodecProvider<AnalyticsContextValues>
{
    /// <inheritdoc />
    public static ICodec<AnalyticsContextValues> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static value => value.Value0,
        Codecs.VarInt,
        static value => value.Value1,
        Codecs.Boolean,
        static value => value.Flag,
        static (value0, value1, flag) => new AnalyticsContextValues(value0, value1, flag)
    );
}
