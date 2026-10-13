using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Chronos;

/// <summary>
/// Defines the Buy Seasonal Catalogue Gift Command contract.
/// </summary>
/// <summary>
/// Defines the Event Id contract.
/// </summary>
/// <summary>
/// Defines the Gift Index contract.
/// </summary>
public sealed record BuySeasonalCatalogueGiftCommand([property: System.Text.Json.Serialization.JsonPropertyName("EventId")] int EventId, int GiftIndex) : CodecCommand<BuySeasonalCatalogueGiftCommand>, ICommandCodecProvider<BuySeasonalCatalogueGiftCommand>
{
    /// <inheritdoc />
    public static CommandCodec<BuySeasonalCatalogueGiftCommand> Codec { get; } = Codecs.Command(
        type: 381,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.EventId,
            Codecs.VarInt,
            static command => command.GiftIndex,
            static (eventId, giftIndex) => new BuySeasonalCatalogueGiftCommand(eventId, giftIndex)
        )
    );
}
