using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events;

/// <summary>Names one run of an event: the event and the variant the player was in.</summary>
/// <param name="EventId">Server number of the event that ran.</param>
/// <param name="VariantId">Which variant of that event the player was in.</param>
public sealed record EventReference(int EventId, int VariantId) : ICodecProvider<EventReference>
{
    /// <inheritdoc />
    public static ICodec<EventReference> Codec { get; } = Codecs.Composite(
        Codecs.Int32,
        static value => value.EventId,
        Codecs.Int32,
        static value => value.VariantId,
        static (eventId, variantId) => new EventReference(eventId, variantId)
    );
}
