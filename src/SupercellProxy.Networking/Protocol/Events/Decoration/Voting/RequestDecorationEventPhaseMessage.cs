using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration.Voting;

/// <summary>Requests the phase state of a decoration event.</summary>
/// <param name="EventId">The event id.</param>
/// <param name="VariantId">The variant id.</param>
/// <param name="Phase">The phase.</param>
public sealed record RequestDecorationEventPhaseMessage(int EventId, int VariantId, int Phase) : CodecMessage<RequestDecorationEventPhaseMessage>, ICodecProvider<RequestDecorationEventPhaseMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestDecorationEventPhaseMessage> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static message => message.EventId,
        Codecs.VarInt,
        static message => message.VariantId,
        Codecs.VarInt,
        static message => message.Phase,
        static (eventId, variantId, phase) => new RequestDecorationEventPhaseMessage(eventId, variantId, phase)
    );
}
