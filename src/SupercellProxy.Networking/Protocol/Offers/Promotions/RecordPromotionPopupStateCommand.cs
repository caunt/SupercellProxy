using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Offers.Promotions;

/// <summary>Records the state of a shown promotion popup.</summary>
/// <param name="PromotionId">The promotion id.</param>
/// <param name="EventId">The event id.</param>
/// <param name="VariantId">The variant id.</param>
/// <param name="State">The state.</param>
/// <param name="Timestamp">The timestamp.</param>
/// <param name="Accepted">The accepted.</param>
/// <param name="Placement">The placement.</param>
public sealed record RecordPromotionPopupStateCommand(string? PromotionId, int EventId, int VariantId, int State, int Timestamp, bool Accepted, int Placement) : CodecCommand<RecordPromotionPopupStateCommand>, ICommandCodecProvider<RecordPromotionPopupStateCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RecordPromotionPopupStateCommand> Codec { get; } = Codecs.Command(
        type: 265,
        Codecs.Composite(
            Codecs.OptionalString,
            static command => command.PromotionId,
            Codecs.VarInt,
            static command => command.EventId,
            Codecs.VarInt,
            static command => command.VariantId,
            Codecs.VarInt,
            static command => command.State,
            Codecs.VarInt,
            static command => command.Timestamp,
            Codecs.Boolean,
            static command => command.Accepted,
            Codecs.VarInt,
            static command => command.Placement,
            static (promotionId, eventId, variantId, state, timestamp, accepted, placement) => new RecordPromotionPopupStateCommand(promotionId, eventId, variantId, state, timestamp, accepted, placement)
        )
    );
}
