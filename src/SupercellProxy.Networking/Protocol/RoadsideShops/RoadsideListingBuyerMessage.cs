using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>
/// Tells a visitor of a roadside shop that one of its stands was bought, so the visitor's copy of the stand shows it as sold.
/// </summary>
/// <param name="SlotIndex">Position of the bought stand; a position past the known stands adds the missing stands first.</param>
/// <param name="BuyerId">Home of the player who bought it; updates naming the shop's own owner are ignored.</param>
/// <param name="HomeOwnerId">Home that owns the shop; updates for other shops are ignored.</param>
public sealed record RoadsideListingBuyerMessage(
    int SlotIndex,
    [property: JsonPropertyName("BuyerId")] LongId BuyerId,
    [property: JsonPropertyName("HomeOwnerId")] LongId HomeOwnerId
) : CodecMessage<RoadsideListingBuyerMessage>, ICodecProvider<RoadsideListingBuyerMessage>
{
    /// <inheritdoc />
    public static ICodec<RoadsideListingBuyerMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            Codecs.VarInt,
            static message => message.SlotIndex,
            Codecs.LongId,
            static message => message.BuyerId,
            Codecs.LongId,
            static message => message.HomeOwnerId,
            static (slotIndex, buyerId, homeOwnerId) => new RoadsideListingBuyerMessage(slotIndex, buyerId, homeOwnerId)
        ),
        trailingDataMessage: "Roadside listing buyer update has trailing data."
    );
}
