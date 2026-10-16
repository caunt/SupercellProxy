using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Offers.Promotions;

/// <summary>Records the state of a shown promotion popup.</summary>
/// <param name="PromotionId">Name of the promotion the popup advertised.</param>
/// <param name="EventId">Live event that carries the promotion.</param>
/// <param name="VariantId">Variant of that event.</param>
/// <param name="State">Popup state being recorded, as native numbers it.</param>
/// <param name="Timestamp">Unix time stamp of the change.</param>
/// <param name="Accepted">Whether the player accepted the popup's offer.</param>
/// <param name="Placement">Where in the game the popup was shown.</param>
public sealed record RecordPromotionPopupStateCommand(string? PromotionId, int EventId, int VariantId, int State, int Timestamp, bool Accepted, int Placement) : CodecCommand<RecordPromotionPopupStateCommand>, ICommandCodecProvider<RecordPromotionPopupStateCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RecordPromotionPopupStateCommand> Codec { get; } = Codecs.Command(
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
