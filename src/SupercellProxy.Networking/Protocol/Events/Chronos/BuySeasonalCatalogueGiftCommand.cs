using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Chronos;

/// <summary>
/// Serverbound command buying one item from a seasonal event's gift catalogue with the event's seasonal currency.
/// </summary>
/// <param name="EventId">Event board event whose catalogue the item is in.</param>
/// <param name="GiftIndex">Position of the item in that event's catalogue, starting at 0.</param>
public sealed record BuySeasonalCatalogueGiftCommand([property: JsonPropertyName("EventId")] int EventId, int GiftIndex) : CodecCommand<BuySeasonalCatalogueGiftCommand>, ICommandCodecProvider<BuySeasonalCatalogueGiftCommand>
{
    /// <inheritdoc />
    public static CommandCodec<BuySeasonalCatalogueGiftCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.EventId,
            Codecs.VarInt,
            static command => command.GiftIndex,
            static (eventId, giftIndex) => new BuySeasonalCatalogueGiftCommand(eventId, giftIndex)
        )
    );
}
