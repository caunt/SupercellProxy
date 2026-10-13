using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Telemetry.Analytics;

/// <summary>One analytics event of a batch, such as <c>connection_failed</c> or <c>chronos_content_download_failed</c>.</summary>
/// <param name="Name">The event name.</param>
/// <param name="Time0">The first time.</param>
/// <param name="Time1">The second time.</param>
/// <param name="Time2">The third time.</param>
/// <param name="Properties">The event properties.</param>
/// <param name="Context">The event context, when present.</param>
public sealed record AnalyticsEvent(string? Name, long Time0, long Time1, long Time2, ReadOnlyMemory<AnalyticsProperty> Properties, AnalyticsContext? Context) : ICodecProvider<AnalyticsEvent>
{
    /// <inheritdoc />
    public static ICodec<AnalyticsEvent> Codec { get; } = Codecs.Composite(
        Codecs.OptionalString,
        static analyticsEvent => analyticsEvent.Name,
        Codecs.Int64,
        static analyticsEvent => analyticsEvent.Time0,
        Codecs.Int64,
        static analyticsEvent => analyticsEvent.Time1,
        Codecs.Int64,
        static analyticsEvent => analyticsEvent.Time2,
        AnalyticsProperty.ListCodec,
        static analyticsEvent => analyticsEvent.Properties,
        Codecs.Optional(AnalyticsContext.Codec),
        static analyticsEvent => analyticsEvent.Context,
        static (name, time0, time1, time2, properties, context) => new AnalyticsEvent(name, time0, time1, time2, properties, context)
    );
}
