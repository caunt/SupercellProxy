using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Telemetry;

/// <summary>Sends a tracking event.</summary>
/// <param name="Category">The category.</param>
/// <param name="Action">The action.</param>
/// <param name="Label">The label.</param>
/// <param name="Data">The data.</param>
/// <param name="Value">The value.</param>
public sealed record TrackingMessage(string? Category, string? Action, string? Label, string? Data, int Value) : CodecMessage<TrackingMessage>, ICodecProvider<TrackingMessage>
{
    /// <inheritdoc />
    public static ICodec<TrackingMessage> Codec { get; } = Codecs.Composite(
        Codecs.OptionalString,
        static message => message.Category,
        Codecs.OptionalString,
        static message => message.Action,
        Codecs.OptionalString,
        static message => message.Label,
        Codecs.OptionalString,
        static message => message.Data,
        Codecs.VarInt,
        static message => message.Value,
        static (category, action, label, data, @value) => new TrackingMessage(category, action, label, data, value)
    );
}
