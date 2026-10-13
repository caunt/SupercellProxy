using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events;

/// <summary>One event reference entry.</summary>
/// <param name="EventId">The event id.</param>
/// <param name="VariantId">The variant id.</param>
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
