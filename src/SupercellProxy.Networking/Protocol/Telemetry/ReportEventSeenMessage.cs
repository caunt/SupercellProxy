using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Telemetry;

/// <summary>Reports that an event was seen.</summary>
/// <param name="EventName">The event name.</param>
/// <param name="EventId">The event id.</param>
/// <param name="VariantId">The variant id.</param>
/// <param name="Source">The source.</param>
/// <param name="Value">The value.</param>
public sealed record ReportEventSeenMessage(string? EventName, int EventId, int VariantId, string? Source, int Value) : CodecMessage<ReportEventSeenMessage>, ICodecProvider<ReportEventSeenMessage>
{
    /// <inheritdoc />
    public static ICodec<ReportEventSeenMessage> Codec { get; } = Codecs.Composite(
        Codecs.OptionalString,
        static message => message.EventName,
        Codecs.VarInt,
        static message => message.EventId,
        Codecs.VarInt,
        static message => message.VariantId,
        Codecs.OptionalString,
        static message => message.Source,
        Codecs.VarInt,
        static message => message.Value,
        static (eventName, eventId, variantId, source, @value) => new ReportEventSeenMessage(eventName, eventId, variantId, source, value)
    );
}
