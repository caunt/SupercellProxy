using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events;

/// <summary>Requests the status of an event.</summary>
/// <param name="EventId">The event id.</param>
public sealed record RequestEventStatusMessage(int EventId) : CodecMessage<RequestEventStatusMessage>, ICodecProvider<RequestEventStatusMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestEventStatusMessage> Codec { get; } = Codecs.Composite(Codecs.VarInt, static message => message.EventId, static (eventId) => new RequestEventStatusMessage(eventId));
}
