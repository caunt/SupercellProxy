using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Telemetry;

/// <summary>Reports that the player saw a live event.</summary>
/// <param name="EventName">Name of the live event.</param>
/// <param name="EventId">Id of the live event.</param>
/// <param name="VariantId">Variant of the live event the player got.</param>
/// <param name="Source">Where the player saw the event; its values are not identified.</param>
/// <param name="Value">Number sent with the report; its meaning is not identified.</param>
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
