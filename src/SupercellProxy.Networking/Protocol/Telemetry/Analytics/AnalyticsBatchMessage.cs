using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Telemetry.Analytics;

/// <summary>Reports a batch of client analytics: session properties such as <c>session_name</c>, an optional context and up to 32 events.</summary>
/// <param name="Time0">Unix time in seconds when the measured session began.</param>
/// <param name="Time1">Total session duration in milliseconds, including intervals when the active session was paused.</param>
/// <param name="Time2">Active session duration in milliseconds, excluding paused intervals. It equals <paramref name="Time1"/> when the session never paused.</param>
/// <param name="Properties">Named details of the batch, such as <c>session_name</c> with launch or Farm_load.</param>
/// <param name="Context">Performance figures measured during the session; null when not sent.</param>
/// <param name="Events">Analytics events recorded during the session, at most 32.</param>
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
