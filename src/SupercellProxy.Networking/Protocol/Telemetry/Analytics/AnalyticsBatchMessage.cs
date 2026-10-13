using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Telemetry.Analytics;

/// <summary>Reports a batch of client analytics: session properties such as <c>session_name</c>, an optional context and up to 32 events.</summary>
/// <param name="Time0">The first time.</param>
/// <param name="Time1">The second time.</param>
/// <param name="Time2">The third time.</param>
/// <param name="Properties">The batch properties.</param>
/// <param name="Context">The batch context, when present.</param>
/// <param name="Events">The events.</param>
public sealed record AnalyticsBatchMessage(
    long Time0,
    long Time1,
    long Time2,
    ReadOnlyMemory<AnalyticsProperty> Properties,
    AnalyticsContext? Context,
    ReadOnlyMemory<AnalyticsEvent> Events
) : CodecMessage<AnalyticsBatchMessage>, ICodecProvider<AnalyticsBatchMessage>
{
    /// <inheritdoc />
    public static ICodec<AnalyticsBatchMessage> Codec { get; } = Codecs.Composite(
        Codecs.Int64,
        static message => message.Time0,
        Codecs.Int64,
        static message => message.Time1,
        Codecs.Int64,
        static message => message.Time2,
        AnalyticsProperty.ListCodec,
        static message => message.Properties,
        Codecs.Optional(AnalyticsContext.Codec),
        static message => message.Context,
        Codecs.Memory(AnalyticsEvent.Codec),
        static message => message.Events,
        static (time0, time1, time2, properties, context, events) => new AnalyticsBatchMessage(time0, time1, time2, properties, context, events)
    );
}
