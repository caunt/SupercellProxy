using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Telemetry;

/// <summary>Sends a tracking event described by a category, an action and a label, in the usual web-analytics shape.</summary>
/// <param name="Category">Group the event belongs to.</param>
/// <param name="Action">What the player did.</param>
/// <param name="Label">What the action was done on.</param>
/// <param name="Data">Extra text sent with the event.</param>
/// <param name="Value">Number sent with the event.</param>
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
