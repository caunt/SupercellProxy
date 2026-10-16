using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Telemetry;

/// <summary>Sends an analytics event with its JSON payload.</summary>
/// <param name="Type">Name of the event, such as gpu_device_info.</param>
/// <param name="Json">Details of the event as a JSON document.</param>
public sealed record AnalyticEventMessage(string? Type, string? Json) : CodecMessage<AnalyticEventMessage>, ICodecProvider<AnalyticEventMessage>
{
    /// <inheritdoc />
    public static ICodec<AnalyticEventMessage> Codec { get; } = Codecs.Composite(
        Codecs.OptionalString,
        static message => message.Type,
        Codecs.OptionalString,
        static message => message.Json,
        static (type, json) => new AnalyticEventMessage(type, json)
    );
}
