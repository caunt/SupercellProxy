using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Telemetry.Analytics;

/// <summary>The first optional analytics context part: battery level at session start and end and a charging flag.</summary>
/// <param name="Value0">Integer battery level captured at session start.</param>
/// <param name="Value1">Integer battery level captured at session end.</param>
/// <param name="Flag">Whether the device was charging when the session state was captured.</param>
/// <remarks>The public CLR property names are retained for compatibility with existing captures and callers.</remarks>
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
