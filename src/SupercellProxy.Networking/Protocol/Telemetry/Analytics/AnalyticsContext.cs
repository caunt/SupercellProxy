using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Telemetry.Analytics;

/// <summary>The optional context native attaches to an analytics batch or event: six presence flags, then each present part.</summary>
/// <param name="Values">The first part.</param>
/// <param name="FirstStatistics">The second part.</param>
/// <param name="SecondStatistics">The third part.</param>
/// <param name="Counters">The fourth part.</param>
/// <param name="Timestamp">The fifth part.</param>
/// <param name="ThirdStatistics">The sixth part.</param>
public sealed record AnalyticsContext(
    AnalyticsContextValues? Values,
    AnalyticsContextStatistics? FirstStatistics,
    AnalyticsContextStatistics? SecondStatistics,
    AnalyticsContextCounters? Counters,
    AnalyticsContextTimestamp? Timestamp,
    AnalyticsContextStatistics? ThirdStatistics
) : ICodecProvider<AnalyticsContext>
{
    /// <inheritdoc />
    /// <remarks>Native writes all six presence flags before the present sections.</remarks>
    public static ICodec<AnalyticsContext> Codec { get; } = Codecs.Create(
        static stream =>
        {
            bool hasValues = stream.ReadBoolean();
            bool hasFirstStatistics = stream.ReadBoolean();
            bool hasSecondStatistics = stream.ReadBoolean();
            bool hasCounters = stream.ReadBoolean();
            bool hasTimestamp = stream.ReadBoolean();
            bool hasThirdStatistics = stream.ReadBoolean();
            AnalyticsContextValues? values = hasValues ? AnalyticsContextValues.Codec.Read(stream) : null;
            AnalyticsContextStatistics? firstStatistics = hasFirstStatistics ? AnalyticsContextStatistics.Codec.Read(stream) : null;
            AnalyticsContextStatistics? secondStatistics = hasSecondStatistics ? AnalyticsContextStatistics.Codec.Read(stream) : null;
            AnalyticsContextCounters? counters = hasCounters ? AnalyticsContextCounters.Codec.Read(stream) : null;
            AnalyticsContextTimestamp? timestamp = hasTimestamp ? AnalyticsContextTimestamp.Codec.Read(stream) : null;

            return new AnalyticsContext(
                values,
                firstStatistics,
                secondStatistics,
                counters,
                timestamp,
                hasThirdStatistics ? AnalyticsContextStatistics.Codec.Read(stream) : null
            );
        },
        static (stream, context) =>
        {
            stream.WriteBoolean(context.Values is not null);
            stream.WriteBoolean(context.FirstStatistics is not null);
            stream.WriteBoolean(context.SecondStatistics is not null);
            stream.WriteBoolean(context.Counters is not null);
            stream.WriteBoolean(context.Timestamp is not null);
            stream.WriteBoolean(context.ThirdStatistics is not null);
            WriteIfPresent(stream, AnalyticsContextValues.Codec, context.Values);
            WriteIfPresent(stream, AnalyticsContextStatistics.Codec, context.FirstStatistics);
            WriteIfPresent(stream, AnalyticsContextStatistics.Codec, context.SecondStatistics);
            WriteIfPresent(stream, AnalyticsContextCounters.Codec, context.Counters);
            WriteIfPresent(stream, AnalyticsContextTimestamp.Codec, context.Timestamp);
            WriteIfPresent(stream, AnalyticsContextStatistics.Codec, context.ThirdStatistics);
        }
    );

    private static void WriteIfPresent<TSection>(ProtocolEncoder encoder, ICodec<TSection> codec, TSection? section) where TSection : class
    {
        if (section is not null)
            codec.Write(encoder, section);
    }
}
